namespace Limaki.UseCases.Viewers.ToolStripViewers {
    public interface ILayoutTool {
        void AttachStyleSheet(string sheetName);
        void DetachStyleSheet(string oldSheetName);
    }
}