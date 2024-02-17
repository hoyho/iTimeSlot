using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using iTimeSlot.Services;

namespace iTimeSlot.MacCatalyst;

public class TrayService : NSObject, ITrayService
{
    /* Read me about interop with macOS:
    https://learn.microsoft.com/en-us/xamarin/ios/internals/objective-c-selectors
    https://developer.apple.com/library/archive/documentation/Cocoa/Conceptual/ProgrammingWithObjectiveC/WorkingwithObjects/WorkingwithObjects.html#//apple_ref/doc/uid/TP40011210-CH4-SW2

    A selector is a message that can be sent to an object or a class
    Unlike normal C functions (and like C++ member functions), 
    you cannot directly invoke a selector using P/Invoke 
    Instead, selectors are sent to an Objective-C class or instance using the objc_msgSend function.
    */

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern IntPtr IntPtr_objc_msgSend_nfloat(IntPtr receiver, IntPtr selector, nfloat arg1);

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern IntPtr IntPtr_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector);

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern void void_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern void void_objc_msgSend_bool(IntPtr receiver, IntPtr selector, bool arg1);

    NSObject systemStatusBarObj;
    NSObject statusBarObj;
    NSObject statusBarItem;
    NSObject statusBarButton;
    NSObject statusBarImage;

    public Action ClickHandler { get; set; }

    public void Initialize()
    {
        //API: https://developer.apple.com/documentation/appkit/nsstatusbar?language=objc
        //statusBarItem = NSStatusBar.systemStatusBar.statusItemWithLength(-1)
        statusBarObj = Runtime.GetNSObject(Class.GetHandle("NSStatusBar"));
        //PerformSelector： Sends a message to the receiver with an object as the argument.
        systemStatusBarObj = statusBarObj.PerformSelector(new Selector("systemStatusBar"));
        statusBarItem = Runtime.GetNSObject(IntPtr_objc_msgSend_nfloat(systemStatusBarObj.Handle, Selector.GetHandle("statusItemWithLength:"), -1));
        
        //statusBarItem.button.image = NSImage.alloc.initWithContentsOfFile(NSBundle.mainBundle.pathForResource_ofType("trayicon", "png"))
        statusBarButton = Runtime.GetNSObject(IntPtr_objc_msgSend(statusBarItem.Handle, Selector.GetHandle("button")));
        statusBarImage = Runtime.GetNSObject(IntPtr_objc_msgSend(ObjCRuntime.Class.GetHandle("NSImage"), Selector.GetHandle("alloc")));

        var imgPath = System.IO.Path.Combine(NSBundle.MainBundle.BundlePath, "Contents", "Resources", "Platforms", "MacCatalyst", "trayicon.png");
        Console.WriteLine(imgPath);
        var imageFileStr = NSString.CreateNative(imgPath);
        var nsImagePtr = IntPtr_objc_msgSend_IntPtr(statusBarImage.Handle, Selector.GetHandle("initWithContentsOfFile:"), imageFileStr);

        void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("setImage:"), statusBarImage.Handle);
        void_objc_msgSend_bool(nsImagePtr, Selector.GetHandle("setTemplate:"), true);

        // Handle click
        void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("setTarget:"), this.Handle);
        void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("setAction:"), new Selector("handleButtonClick:").Handle);
    }

    [Export("handleButtonClick:")]
    void HandleClick(NSObject senderStatusBarButton)
    {
        var nsapp = Runtime.GetNSObject(Class.GetHandle("NSApplication"));
        var sharedApp = nsapp.PerformSelector(new Selector("sharedApplication"));

        void_objc_msgSend_bool(sharedApp.Handle, Selector.GetHandle("activateIgnoringOtherApps:"), true);

        ClickHandler?.Invoke();
    }
}