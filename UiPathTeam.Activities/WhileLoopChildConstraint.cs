using System.Activities;
using System.Activities.Validation;
using System.Collections.Generic;
using System.ComponentModel;

namespace UiPathTeam.Activities
{
    internal class WhileLoopChildConstraint : NativeActivity
    {

        #region Properties

        [RequiredArgument]
        [DefaultValue(null)]
        public InArgument<IEnumerable<Activity>> ParentChain { get; set; }

        #endregion

        #region CodeActivity

        protected override void Execute(NativeActivityContext context)
        {
            foreach (Activity act in ParentChain.Get(context))
            {
                if (IsWhileLoop(act))
                    return;
            }
            Constraint.AddValidationError(context, new ValidationError("Activity can only be placed inside a WhileLoop", false));
        }

        #endregion

        #region HelperMethods

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument("ParentChain", typeof(IEnumerable<Activity>), ArgumentDirection.In, true);
            metadata.Bind(ParentChain, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        private bool IsWhileLoop(Activity act)
        {
            if (act == null)
                return false;
            if (act.GetType().IsGenericType)
                return act.GetType().GetGenericTypeDefinition().Equals(typeof(WhileLoop));
            if (act.GetType().BaseType != null && act.GetType().BaseType.IsGenericType)
                return act.GetType().BaseType.GetGenericTypeDefinition().Equals(typeof(WhileLoop));
            return false;
        }

        #endregion

    }
}