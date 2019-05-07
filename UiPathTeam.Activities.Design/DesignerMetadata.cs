using System.Activities.Presentation.Metadata;
using System.ComponentModel;

namespace UiPathTeam.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            // Designers for DoWhileLoop
            builder.AddCustomAttributes(typeof(DoWhileLoop), new DesignerAttribute(typeof(DoWhileLoopActivityDesigner)));

            // Designers for WhileLoop
            builder.AddCustomAttributes(typeof(WhileLoop), new DesignerAttribute(typeof(WhileLoopActivityDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
