using Telerik.WinControls.UI;

namespace KarveControls.KarveGrid.Column.Types
{
    public class DataGridCheckBoxColumn : GridViewCheckBoxColumn, IMixinDataBaseExtension
    {

        public int Item { get => MDataBaseExtension.GetItem(this); set => MDataBaseExtension.SetItem(this, value); }
        public string ExtendedFieldName { get => MDataBaseExtension.GetExtendedFieldName(this); set => MDataBaseExtension.SetExtendedFieldName(this, value); }
        public string Table { get => MDataBaseExtension.GetTable(this); set => MDataBaseExtension.SetTable(this, value); }
        public string ExpressionDb { get => MDataBaseExtension.GetExpressionDb(this); set => MDataBaseExtension.SetTable(this, value); }
        public System.Drawing.Color BackGroundColor { get => MDataBaseExtension.GetBackgroundColor(this); set => MDataBaseExtension.SetBackgroundColor(this, value); }
        public string AliasCampo {
            get => FieldName; set => FieldName = value;
        }
        public override bool ReadOnly {
            get => base.ReadOnly; set {
                base.ReadOnly = value;
                if (value) {
                    BackGroundColor = DefaultColors.DefaultReadOnlyColor;
                }
            }
        }
    }
}
