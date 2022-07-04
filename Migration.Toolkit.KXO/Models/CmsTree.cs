using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Tree")]
    [Index("NodeAliasPath", Name = "IX_CMS_Tree_NodeAliasPath")]
    [Index("NodeClassId", Name = "IX_CMS_Tree_NodeClassID")]
    [Index("NodeLevel", Name = "IX_CMS_Tree_NodeLevel")]
    [Index("NodeLinkedNodeId", Name = "IX_CMS_Tree_NodeLinkedNodeID")]
    [Index("NodeLinkedNodeSiteId", Name = "IX_CMS_Tree_NodeLinkedNodeSiteID")]
    [Index("NodeOriginalNodeId", Name = "IX_CMS_Tree_NodeOriginalNodeID")]
    [Index("NodeOwner", Name = "IX_CMS_Tree_NodeOwner")]
    [Index("NodeParentId", "NodeAlias", "NodeName", Name = "IX_CMS_Tree_NodeParentID_NodeAlias_NodeName")]
    [Index("NodeSiteId", "NodeGuid", Name = "IX_CMS_Tree_NodeSiteID_NodeGUID", IsUnique = true)]
    public partial class CmsTree
    {
        public CmsTree()
        {
            CmsDocuments = new HashSet<CmsDocument>();
            CmsPageFormerUrlPaths = new HashSet<CmsPageFormerUrlPath>();
            CmsPageUrlPaths = new HashSet<CmsPageUrlPath>();
            InverseNodeLinkedNode = new HashSet<CmsTree>();
            InverseNodeOriginalNode = new HashSet<CmsTree>();
            InverseNodeParent = new HashSet<CmsTree>();
        }

        [Key]
        [Column("NodeID")]
        public int NodeId { get; set; }
        public string NodeAliasPath { get; set; } = null!;
        [StringLength(100)]
        public string NodeName { get; set; } = null!;
        [StringLength(50)]
        public string NodeAlias { get; set; } = null!;
        [Column("NodeClassID")]
        public int NodeClassId { get; set; }
        [Column("NodeParentID")]
        public int? NodeParentId { get; set; }
        public int NodeLevel { get; set; }
        [Column("NodeSiteID")]
        public int NodeSiteId { get; set; }
        [Column("NodeGUID")]
        public Guid NodeGuid { get; set; }
        public int? NodeOrder { get; set; }
        public bool? IsSecuredNode { get; set; }
        [Column("NodeLinkedNodeID")]
        public int? NodeLinkedNodeId { get; set; }
        public int? NodeOwner { get; set; }
        public string? NodeCustomData { get; set; }
        [Column("NodeLinkedNodeSiteID")]
        public int? NodeLinkedNodeSiteId { get; set; }
        public bool? NodeHasChildren { get; set; }
        public bool? NodeHasLinks { get; set; }
        [Column("NodeOriginalNodeID")]
        public int? NodeOriginalNodeId { get; set; }

        [ForeignKey("NodeClassId")]
        [InverseProperty("CmsTrees")]
        public virtual CmsClass NodeClass { get; set; } = null!;
        [ForeignKey("NodeLinkedNodeId")]
        [InverseProperty("InverseNodeLinkedNode")]
        public virtual CmsTree? NodeLinkedNode { get; set; }
        [ForeignKey("NodeLinkedNodeSiteId")]
        [InverseProperty("CmsTreeNodeLinkedNodeSites")]
        public virtual CmsSite? NodeLinkedNodeSite { get; set; }
        [ForeignKey("NodeOriginalNodeId")]
        [InverseProperty("InverseNodeOriginalNode")]
        public virtual CmsTree? NodeOriginalNode { get; set; }
        [ForeignKey("NodeOwner")]
        [InverseProperty("CmsTrees")]
        public virtual CmsUser? NodeOwnerNavigation { get; set; }
        [ForeignKey("NodeParentId")]
        [InverseProperty("InverseNodeParent")]
        public virtual CmsTree? NodeParent { get; set; }
        [ForeignKey("NodeSiteId")]
        [InverseProperty("CmsTreeNodeSites")]
        public virtual CmsSite NodeSite { get; set; } = null!;
        [InverseProperty("DocumentNode")]
        public virtual ICollection<CmsDocument> CmsDocuments { get; set; }
        [InverseProperty("PageFormerUrlPathNode")]
        public virtual ICollection<CmsPageFormerUrlPath> CmsPageFormerUrlPaths { get; set; }
        [InverseProperty("PageUrlPathNode")]
        public virtual ICollection<CmsPageUrlPath> CmsPageUrlPaths { get; set; }
        [InverseProperty("NodeLinkedNode")]
        public virtual ICollection<CmsTree> InverseNodeLinkedNode { get; set; }
        [InverseProperty("NodeOriginalNode")]
        public virtual ICollection<CmsTree> InverseNodeOriginalNode { get; set; }
        [InverseProperty("NodeParent")]
        public virtual ICollection<CmsTree> InverseNodeParent { get; set; }
    }
}
