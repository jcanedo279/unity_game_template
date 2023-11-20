public interface IUIObjectWithClick {
    public System.Action<string> OnClickUIObjectDelegate { get; set; }
    public void OnClickUIObject();
}
