package md5bab1d5034739274ea386e5fc4fc5923b;


public class UsbSerialRuntimeException
	extends java.lang.RuntimeException
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Hoho.Android.UsbSerial.Driver.UsbSerialRuntimeException, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", UsbSerialRuntimeException.class, __md_methods);
	}


	public UsbSerialRuntimeException () throws java.lang.Throwable
	{
		super ();
		if (getClass () == UsbSerialRuntimeException.class)
			mono.android.TypeManager.Activate ("Hoho.Android.UsbSerial.Driver.UsbSerialRuntimeException, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public UsbSerialRuntimeException (java.lang.String p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == UsbSerialRuntimeException.class)
			mono.android.TypeManager.Activate ("Hoho.Android.UsbSerial.Driver.UsbSerialRuntimeException, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public UsbSerialRuntimeException (java.lang.String p0, java.lang.Throwable p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == UsbSerialRuntimeException.class)
			mono.android.TypeManager.Activate ("Hoho.Android.UsbSerial.Driver.UsbSerialRuntimeException, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Java.Lang.Throwable, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public UsbSerialRuntimeException (java.lang.Throwable p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == UsbSerialRuntimeException.class)
			mono.android.TypeManager.Activate ("Hoho.Android.UsbSerial.Driver.UsbSerialRuntimeException, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Java.Lang.Throwable, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
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
