package md5bab1d5034739274ea386e5fc4fc5923b;


public class UsbSupport
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Hoho.Android.UsbSerial.Driver.UsbSupport, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", UsbSupport.class, __md_methods);
	}


	public UsbSupport () throws java.lang.Throwable
	{
		super ();
		if (getClass () == UsbSupport.class)
			mono.android.TypeManager.Activate ("Hoho.Android.UsbSerial.Driver.UsbSupport, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
