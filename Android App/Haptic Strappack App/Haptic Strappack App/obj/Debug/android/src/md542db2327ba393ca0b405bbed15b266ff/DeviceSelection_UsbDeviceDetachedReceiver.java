package md542db2327ba393ca0b405bbed15b266ff;


public class DeviceSelection_UsbDeviceDetachedReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Haptic_Strappack_App.DeviceSelection+UsbDeviceDetachedReceiver, Haptic Strappack App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DeviceSelection_UsbDeviceDetachedReceiver.class, __md_methods);
	}


	public DeviceSelection_UsbDeviceDetachedReceiver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DeviceSelection_UsbDeviceDetachedReceiver.class)
			mono.android.TypeManager.Activate ("Haptic_Strappack_App.DeviceSelection+UsbDeviceDetachedReceiver, Haptic Strappack App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public DeviceSelection_UsbDeviceDetachedReceiver (md542db2327ba393ca0b405bbed15b266ff.DeviceSelection p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == DeviceSelection_UsbDeviceDetachedReceiver.class)
			mono.android.TypeManager.Activate ("Haptic_Strappack_App.DeviceSelection+UsbDeviceDetachedReceiver, Haptic Strappack App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Haptic_Strappack_App.DeviceSelection, Haptic Strappack App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
