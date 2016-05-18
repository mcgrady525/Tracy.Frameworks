namespace Tracy.Frameworks.Redis
{
    public class DebugLogger : System.IO.StringWriter
    {
        public override void WriteLine()
        {

            System.Diagnostics.Debug.WriteLine("");
        }
        public override void WriteLine(string value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }
        public override void WriteLine(object value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }
        public override void WriteLine(string format, object arg0)
        {
            System.Diagnostics.Debug.WriteLine(format, arg0);
        }
        public override void WriteLine(string format, object arg0, object arg1)
        {
            System.Diagnostics.Debug.WriteLine(format, arg0, arg1);
        }
        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            System.Diagnostics.Debug.WriteLine(format, arg0, arg1, arg2);
        }
        public override void WriteLine(string format, params object[] arg)
        {
            System.Diagnostics.Debug.WriteLine(format, arg);
        }
    }
}
