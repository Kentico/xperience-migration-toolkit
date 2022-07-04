using System.Collections;
using System.Diagnostics;

namespace Migration.Toolkit.Common.Helpers;

public static class EnumerableHelper
{
    public static SimpleAligner<TA, TB, TKey> CreateAligner<TA, TB, TKey>(IEnumerable<TA> eA, IEnumerable<TB> eB, IEnumerable<TKey> eK,
        SimpleAligner<TA, TB, TKey>.SelectKey<TA> selectKeyA, SimpleAligner<TA, TB, TKey>.SelectKey<TB> selectKeyB, bool disposeEnumerators)
        where TA : class where TB : class => SimpleAligner<TA, TB, TKey>.Create(eA.GetEnumerator(), eB.GetEnumerator(), eK.GetEnumerator(), selectKeyA, selectKeyB, disposeEnumerators);
}

public class Aligner<TA, TB, TKey>: IEnumerable<SimpleAlignResult<TA, TB, TKey>> where TA : class where TB : class {
    private readonly IEnumerator<TA> _eA;
    private readonly IEnumerator<TB> _eB;
    private readonly IEnumerator<TKey> _eK;
    private readonly SimpleAligner<TA, TB, TKey>.SelectKey<TA> _selectKeyA;
    private readonly SimpleAligner<TA, TB, TKey>.SelectKey<TB> _selectKeyB;
    private readonly bool _disposeEnumerators;

    public Aligner(IEnumerator<TA> eA, 
        IEnumerator<TB> eB, 
        IEnumerator<TKey> eK,
        SimpleAligner<TA, TB, TKey>.SelectKey<TA> selectKeyA,
        SimpleAligner<TA, TB, TKey>.SelectKey<TB> selectKeyB,
        bool disposeEnumerators)
    {
        _eA = eA;
        _eB = eB;
        _eK = eK;
        _selectKeyA = selectKeyA;
        _selectKeyB = selectKeyB;
        _disposeEnumerators = disposeEnumerators;
    }
    
    public IEnumerator<SimpleAlignResult<TA, TB, TKey>> GetEnumerator()
    {
        return SimpleAligner<TA, TB, TKey>.Create(_eA, _eB, _eK,_selectKeyA, _selectKeyB, _disposeEnumerators);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class SimpleAligner<TA, TB, TKey> : IEnumerator<SimpleAlignResult<TA, TB, TKey>> where TA : class where TB : class
{
        public delegate TKey? SelectKey<in T>(T? current);

        private readonly SelectKey<TA> _selectKeyA;
        private readonly SelectKey<TB> _selectKeyB;
        private readonly bool _disposeEnumerators;
        private readonly IEnumerator<TA> _eA;
        private readonly IEnumerator<TB> _eB;
        private readonly IEnumerator<TKey> _eK;
        public int Ordinal { get; private set; }

        private SimpleAligner(
            IEnumerator<TA> eA,
            IEnumerator<TB> eB,
            IEnumerator<TKey> eK,
            SelectKey<TA> selectKeyA,
            SelectKey<TB> selectKeyB,
            bool disposeEnumerators
        )
        {
            _selectKeyA = selectKeyA;
            _selectKeyB = selectKeyB;
            _disposeEnumerators = disposeEnumerators;
            _eA = eA;
            _eB = eB;
            _eK = eK;
            
            
            // this._streamWriter = new StreamWriter("/home/admin77/RiderProjects/Kensync/Output/pairing.txt");
        }

        public static SimpleAligner<TA, TB, TKey> Create(
            IEnumerator<TA> eA, 
            IEnumerator<TB> eB, 
            IEnumerator<TKey> eK,
            SelectKey<TA> selectKeyA,
            SelectKey<TB> selectKeyB,
            bool disposeEnumerators
            )
        {
            return new SimpleAligner<TA, TB, TKey>(eA, eB, eK, selectKeyA, selectKeyB, disposeEnumerators);
        }

        private bool _hasA;
        private bool _hasB;
        private bool _hasK = true;

        private bool _firstMove = true;
        private readonly StreamWriter _streamWriter;

        public bool MoveNext()
        {
            if (_firstMove)
            {
                _hasA = _eA.MoveNext();
                _hasB = _eB.MoveNext();
                _firstMove = false;
            }

            _hasK = _eK.MoveNext();
            if (!_hasK) return false;

            do
            {
                var keyA = _selectKeyA(_eA.Current);
                var keyB = _selectKeyB(_eB.Current);

                var matchA = _eA.Current != default && Object.Equals(keyA, _eK.Current);
                var matchB = _eB.Current != default && Object.Equals(keyB, _eK.Current);
                if (matchA && matchB)
                {
                    Trace.WriteLine($"AB.Match: K={_eK.Current} => yield result");
                    Current = new SimpleAlignResultMatch<TA?, TB?, TKey>(_eA.Current, _eB.Current, _eK.Current);
                    
                    _streamWriter?.WriteLine($"MATCH      {_eK.Current}{keyA}{keyB}");
                    
                    _hasA = _hasA && _eA.MoveNext();
                    _hasB = _hasB && _eB.MoveNext();
                    Ordinal++;
                    return _hasK;
                }

                if (matchA)
                {
                    Trace.WriteLine($"A .Match: K={_eK.Current} => yield result");
                    Current = new SimpleAlignResultOnlyA<TA?, TB?, TKey>(_eA.Current, _eK.Current);
                    
                    _streamWriter?.WriteLine($"MATCH-A    {_eK.Current}{keyA}{keyB}");
                    
                    _hasA = _hasA && _eA.MoveNext();
                    Ordinal++;
                    return _hasK;
                }

                if (matchB)
                {
                    Trace.WriteLine($"B .Match: K={_eK.Current} => yield result");
                    Current = new SimpleAlignResultOnlyB<TA?, TB?, TKey>(_eB.Current, _eK.Current);
                    
                    _streamWriter?.WriteLine($"MATCH-B    {_eK.Current}{keyA}{keyB}");
                    
                    _hasB = _hasB && _eB.MoveNext();
                    Ordinal++;
                    return _hasK;
                }

                if (!matchA && !matchB)
                {
                    Trace.WriteLine($"AB.MatchNot: K={_eK.Current} AK={keyA} BK={keyB}");
                    Current = new SimpleAlignFatalNoMatch<TA?, TB?, TKey>(_eA.Current, _eB.Current, _eK.Current, "AB.NOMATCH: possibly error / wrongly sorted, selected source enumerators.");
                    
                    _streamWriter?.WriteLine($"MATCH-NOPE {_eK.Current}{keyA}{keyB}");
                    
                    Ordinal++;
                    return _hasK;
                    // throw new InvalidOperationException($"AB.NOMATCH: possibly error / wrongly sorted, selected source enumerators. Key: {CurrentKey}");
                }
            } while (_hasA || _hasB);

            return false;
        }

        public void Reset()
        {
            _eA.Reset();
            _eB.Reset();
            _eK.Reset();
        }

        public SimpleAlignResult<TA?, TB?, TKey> Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _streamWriter?.Dispose();
            if (!_disposeEnumerators) return;
            _eA?.Dispose();
            _eB?.Dispose();
            _eK?.Dispose();
        }
    }