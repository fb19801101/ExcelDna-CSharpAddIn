using ExcelDna.Integration;
using ExcelDna.IntelliSense;

namespace CSharpAddIn
{
    class XllAddIn : IExcelAddIn
    {
        //https://msdn.microsoft.com/zh-cn/library/bb687860(v=office.15)
        //Callback function that must be implemented and exported by every valid XLL. The xlAutoOpen function is the recommended place from where to register XLL functions and commands, initialize data structures, customize the user interface, and so on.
        public void AutoOpen()
        {
            IntelliSenseServer.Install();
            //ExcelDna.Logging.LogDisplay.Show();
            //ExcelDna.Logging.LogDisplay.DisplayOrder = ExcelDna.Logging.DisplayOrder.NewestFirst;
        }

        //https://msdn.microsoft.com/zh-cn/library/bb687830.aspx
        //Called by Microsoft Excel whenever the XLL is deactivated. The add-in is deactivated when an Excel session ends normally. The add-in can be deactivated by the user during an Excel session, and this function will be called in that case.
        //Excel does not require an XLL to implement and export this function, although it is advisable so that your XLL can unregister functions and commands, release resources, undo customizations, and so on.If functions and commands are not explicitly unregistered by the XLL, Excel does this after calling the xlAutoClose function.
        public void AutoClose()
        {
            // The explicit Uninstall call was added in version 1.0.10, 
            // to give us a chance to unhook the WinEvents (which needs to happen on the main thread)
            // No easy plan to do this from the AppDomain unload event, which runs on a ThreadPool thread.
            IntelliSenseServer.Uninstall();
        }
    }
}
