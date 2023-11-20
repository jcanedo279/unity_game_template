public interface IUIObjectWithStringValueClick {
    public System.Action<string,string> OnClickUIObjectDelegate { get; set; }
    public void OnClickUIObject();
}
