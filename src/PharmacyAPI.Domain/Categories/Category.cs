using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.Categories
{
    public class Category : FullAuditedAggregateRoot<Guid>
    {
        public virtual string CategoryName{get; set;}
         public const int CategoryNameMaxLength=128;
        public const int CategoryNameMinLength=2;

        // parentid sunun icin->
        //simdi categorylere bolunmesi gerek enfeksiyon bas agrisi vs. falan
        // alt categorylere de ayrilmasi icin
        public Guid? ParentId{get;set;}

        public Category Parent{get; set;}

        
        // bu da listelemek icin var
        public ICollection<Category> Childeren {get; set;} = new List<Category>();
        protected Category(){}

        public Category(Guid id, string name, Guid? parentId=null): base(id)
        {
            CategoryName=name;
            ParentId=parentId;
        }
        public virtual void SetCategoryName(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(CategoryName),CategoryNameMaxLength,CategoryNameMinLength);
            CategoryName=name;
        }

    }
}