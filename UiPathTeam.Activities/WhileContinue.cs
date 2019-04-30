using System.Activities;
using System.Activities.Statements;
using System.Activities.Validation;

namespace UiPathTeam.Activities
{
    public class WhileContinue : NativeActivity
    {

        #region Properties

        protected override bool CanInduceIdle { get { return true; } }

        #endregion

        #region CodeActivity

        protected override void Execute(NativeActivityContext context)
        {
            Bookmark continueProperty = (Bookmark)context.Properties.Find("ContinueBookmark");
            Bookmark willNeverBeResumed = context.CreateBookmark();
            var result = context.ResumeBookmark(continueProperty, willNeverBeResumed);
        }

        #endregion
        
        #region CheckParent

        public WhileContinue()
        {
            base.Constraints.Add(CheckParent());
        }

        static Constraint CheckParent()
        {
            DelegateInArgument<WhileContinue> element = new DelegateInArgument<WhileContinue>();
            DelegateInArgument<ValidationContext> context = new DelegateInArgument<ValidationContext>();
            Variable<bool> result = new Variable<bool>();
            DelegateInArgument<Activity> parent = new DelegateInArgument<Activity>();

            return new Constraint<WhileContinue>
            {
                Body = new ActivityAction<WhileContinue, ValidationContext>
                {
                    Argument1 = element,
                    Argument2 = context,
                    Handler = new Sequence
                    {
                        Variables =
                    {
                        result
                    },
                        Activities =
                    {
                        new ForEach<Activity>
                        {
                            Values = new GetParentChain
                            {
                                ValidationContext = context
                            },
                            Body = new ActivityAction<Activity>
                            {
                                Argument = parent,
                                Handler = new If()
                                {
                                    Condition = new InArgument<bool>((env) => object.Equals(parent.Get(env).GetType(),typeof(WhileLoop))),
                                    Then = new Assign<bool>
                                    {
                                        Value = true,
                                        To = result
                                    }
                                }
                            }
                        },
                        new AssertValidation
                        {
                            Assertion = new InArgument<bool>(result),
                            Message = new InArgument<string> ("WhileContinue has to be inside a WhileLoop activity"),
                        }
                    }
                    }
                }
            };
        }

        #endregion

    }
}
