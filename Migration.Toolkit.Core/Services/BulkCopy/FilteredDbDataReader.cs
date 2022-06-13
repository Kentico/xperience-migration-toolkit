using System.Data;
using System.Data.Common;

namespace Migration.Toolkit.Core.Services.BulkCopy;

public class FilteredDbDataReader<TReader> : DataReaderProxyBase where TReader: IDataReader
{
    private readonly Func<IDataReader, bool> _includePredicate;
    public int TotalItems { get; private set; } = 0;
    public int TotalNonFiltered { get; private set; } = 0;

    public FilteredDbDataReader(IDataReader innerReader, Func<IDataReader, bool> includePredicate) : base(innerReader)
    {
        _includePredicate = includePredicate;
    }

    public override bool Read()
    {
        while (true)
        {
            if (base.Read())
            {
                TotalItems++;
                if (!_includePredicate(_innerReader))
                {
                    continue;
                }

                TotalNonFiltered++;
                return true;
            }

            return false;
        }
    }
}