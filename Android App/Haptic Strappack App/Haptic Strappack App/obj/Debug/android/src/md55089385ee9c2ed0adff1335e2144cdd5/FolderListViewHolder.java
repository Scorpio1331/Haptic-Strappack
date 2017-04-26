package md55089385ee9c2ed0adff1335e2144cdd5;


public class FolderListViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("haptic_strappack_app.FolderListViewHolder, Haptic Strappack App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FolderListViewHolder.class, __md_methods);
	}


	public FolderListViewHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FolderListViewHolder.class)
			mono.android.TypeManager.Activate ("haptic_strappack_app.FolderListViewHolder, Haptic Strappack App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public FolderListViewHolder (android.widget.TextView p0, android.widget.ImageView p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == FolderListViewHolder.class)
			mono.android.TypeManager.Activate ("haptic_strappack_app.FolderListViewHolder, Haptic Strappack App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.ImageView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
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
