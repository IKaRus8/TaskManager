
public class TempPanel : BasePanel
{
    public override void Show()
    {
        base.Show();
    }

    public override void Close()
    {
        Destroy(this.gameObject);
    }
}
