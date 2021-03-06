﻿using System;
using System.Activities;
using System.ComponentModel;
using System.Windows.Markup;

namespace UiPathTeam.Core.Activities
{
    [ContentProperty("Body"), DisplayName("Do While Loop"), Description("DoWhile loop with an associated Break (While Break) and Continue (While Continue)"), Obsolete("Break and Continue activities within While/Do While loops have been integrated into 2020.4", false)]
    public class DoWhileLoop : NativeActivity
    {

        #region Properties

        [DefaultValue(null)]
        public Activity<bool> Condition { get; set; }

        [Browsable(false)]
        public ActivityAction Body { get; set; }

        protected override bool CanInduceIdle { get { return true; } }

        #endregion

        #region CodeActivity

        public DoWhileLoop()
        {
            Condition = null;
            Body = new ActivityAction
            {
                DisplayName = "Body"
            };
        }

        protected override void Execute(NativeActivityContext context)
        {
            Bookmark continueBookmark = context.CreateBookmark(OnContinue, BookmarkOptions.MultipleResume | BookmarkOptions.NonBlocking);
            Bookmark breakBookmark = context.CreateBookmark(OnBreak, BookmarkOptions.NonBlocking);

            context.Properties.Add("ContinueBookmark", continueBookmark);
            context.Properties.Add("BreakBookmark", breakBookmark);
            context.ScheduleAction(Body, BodyCompletion, null);
        }

        #endregion

        #region ControlMethods

        private void ConditionCompletion(NativeActivityContext context, ActivityInstance completedInstance, bool result)
        {
            if (completedInstance.State == ActivityInstanceState.Closed && result)
            {
                context.ScheduleAction(Body, BodyCompletion, null);
            }
        }

        private void BodyCompletion(NativeActivityContext context, ActivityInstance completedInstance)
        {
            if (completedInstance.State == ActivityInstanceState.Closed)
            {
                context.ScheduleActivity(Condition, ConditionCompletion);
            }
        }

        private void OnContinue(NativeActivityContext context, Bookmark bookmark, object value)
        {
            context.CancelChildren();
            context.ScheduleActivity(Condition, ConditionCompletion);
        }

        private void OnBreak(NativeActivityContext context, Bookmark bookmark, object value)
        {
            context.CancelChildren();
        }

        #endregion

        #region CheckCondition

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if(Condition == null)
            {
                metadata.AddValidationError($"Condition must be set before DoWhileLoop activity '{ DisplayName }' can be used.");
            }
        }

        #endregion

    }
}
