﻿using System;
using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;
using System.Activities.Validation;

namespace UiPathTeam.Core.Activities
{
    [DisplayName("While Break"), Description("Break out of a Do While Loop or a While Loop, ending the execution of that loop"), Obsolete("Break and Continue activities within While/Do While loops have been integrated into 2020.4", false)]
    public class WhileBreak : NativeActivity
    {

        #region Properties

        protected override bool CanInduceIdle { get { return true; } }

        #endregion

        #region CodeActivity

        protected override void Execute(NativeActivityContext context)
        {
            Bookmark breakProperty = (Bookmark)context.Properties.Find("BreakBookmark");
            Bookmark willNeverBeResumed = context.CreateBookmark();
            var result = context.ResumeBookmark(breakProperty, willNeverBeResumed);
        }

        #endregion

        #region CheckParent

        public WhileBreak()
        {
            base.Constraints.Add(CheckParent());
        }

        private static Constraint CheckParent()
        {
            DelegateInArgument<WhileBreak> element = new DelegateInArgument<WhileBreak>();
            DelegateInArgument<ValidationContext> context = new DelegateInArgument<ValidationContext>();
            Variable<bool> result = new Variable<bool>();
            DelegateInArgument<Activity> parent = new DelegateInArgument<Activity>();

            return new Constraint<WhileBreak>
            {
                Body = new ActivityAction<WhileBreak, ValidationContext>
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
                                    Condition = new InArgument<bool>((env) => (object.Equals(parent.Get(env).GetType(),typeof(DoWhileLoop)) || object.Equals(parent.Get(env).GetType(),typeof(WhileLoop)))),
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
                            Message = new InArgument<string> ("WhileBreak has to be inside a DoWhileLoop or WhileLoop activity"),
                        }
                    }
                    }
                }
            };
        }

        #endregion

    }
}
