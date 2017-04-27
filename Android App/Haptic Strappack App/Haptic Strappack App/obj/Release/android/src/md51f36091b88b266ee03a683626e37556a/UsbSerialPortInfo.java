package md51f36091b88b266ee03a683626e37556a;


public class UsbSerialPortInfo
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.os.Parcelable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_GetCreator:()Landroid/os/Parcelable$Creator;:__export__\n" +
			"n_describeContents:()I:GetDescribeContentsHandler:Android.OS.IParcelableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_writeToParcel:(Landroid/os/Parcel;I)V:GetWriteToParcel_Landroid_os_Parcel_IHandler:Android.OS.IParcelableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Hoho.Android.UsbSerial.Extensions.UsbSerialPortInfo, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", UsbSerialPortInfo.class, __md_methods);
	}


	public UsbSerialPortInfo () throws java.lang.Throwable
	{
		super ();
		if (getClass () == UsbSerialPortInfo.class)
			mono.android.TypeManager.Activate ("Hoho.Android.UsbSerial.Extensions.UsbSerialPortInfo, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public UsbSerialPortInfo (android.os.Parcel p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == UsbSerialPortInfo.class)
			mono.android.TypeManager.Activate ("Hoho.Android.UsbSerial.Extensions.UsbSerialPortInfo, Android.UsbSerial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Parcel, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public static android.os.Parcelable.Creator CREATOR = GetCreator ();

	public static android.os.Parcelable.Creator GetCreator ()
	{
		return n_GetCreator ();
	}

	private static native android.os.Parcelable.Creator n_GetCreator ();


	public int describeContents ()
	{
		return n_describeContents ();
	}

	private native int n_describeContents ();


	public void writeToParcel (android.os.Parcel p0, int p1)
	{
		n_writeToParcel (p0, p1);
	}

	private native void n_writeToParcel (android.os.Parcel p0, int p1);

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
