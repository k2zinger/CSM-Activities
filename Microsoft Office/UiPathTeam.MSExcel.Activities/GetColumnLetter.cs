using System;
using System.Activities;
using System.ComponentModel;
using System.Data;

namespace UiPathTeam.MSExcel.Activities
{
    [DisplayName("Get Column Letter"), Description("Get the Column Letter within an Excel Worksheet by providing a DataTable and Column Name (case sensitive) or Index of Column (starts with index 1)")]
    public class GetColumnLetter : CodeActivity
    {
        #region Properties

        [Category("Input"), Description("Name of Column (exact match)")]
        [RequiredArgument]
        [OverloadGroup("G1")]
        public InArgument<string> ColumnName { get; set; }

        [Category("Input"), Description("Index of Column (starting with index 1)")]
        [RequiredArgument]
        [OverloadGroup("G2")]
        public InArgument<int> ColumnNumber { get; set; }

        [Category("Input"), Description("DataTable to obtain Column Letter from")]
        [RequiredArgument]
        public InArgument<DataTable> DataTable { get; set; }

        [Category("Output"), Description("Column Letter in Excel (e.g. A, BZ, etc.)")]
        public OutArgument<string> ColumnLetter { get; set; }

        #endregion

        #region CodeActivity

        protected override void Execute(CodeActivityContext context)
        {
            string cn = ColumnName.Get(context);
            DataTable dt = DataTable.Get(context);
            int ci;

            if (null == cn)
            {
                ci = ColumnNumber.Get(context);
            }
            else
            {
                if (!dt.Columns.Contains(cn))
                {
                    throw new ArgumentException("Column '" + cn + "' was not found");
                }

                //add 1 to the column index since we always start with column index 0
                ci = dt.Columns.IndexOf(cn) + 1;
            }

            ColumnLetter.Set(context, CalculateColumnLetter(ci));
        }

        #endregion

        #region HelperMethods

        public static string CalculateColumnLetter(int columnIndex)
        {
            int temp = 0;
            string columnLetter = String.Empty;
            while (columnIndex > 0)
            {
                temp = (columnIndex - 1) % 26;
                columnLetter = (char)(65 + temp) + columnLetter;
                columnIndex = (int)((columnIndex - temp) / 26);
            }

            return columnLetter;
        }

        #endregion

    }
}