// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Szyfrator2
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton CipherButton { get; set; }

		[Outlet]
		AppKit.NSTextField CipheredDataTextField { get; set; }

		[Outlet]
		AppKit.NSTextField CipheredOutputTextField { get; set; }

		[Outlet]
		AppKit.NSTextField CipherKeyTextField { get; set; }

		[Outlet]
		AppKit.NSButton DecipherButton { get; set; }

		[Outlet]
		AppKit.NSTextField DecipherKeyTextField { get; set; }

		[Outlet]
		AppKit.NSTabView MainTabView { get; set; }

		[Outlet]
		AppKit.NSTextField RawDataTextField { get; set; }

		[Outlet]
		AppKit.NSTextField RawOutputTextField { get; set; }

		[Action ("aaa:")]
		partial void aaa (AppKit.NSTextField sender);

		[Action ("dupadupadupa:")]
		partial void dupadupadupa (AppKit.NSMenuItem sender);

		[Action ("newCipher:")]
		partial void newCipher (AppKit.NSMenuItem sender);

		[Action ("openCipher:")]
		partial void openCipher (AppKit.NSMenuItem sender);

		[Action ("openText:")]
		partial void openText (AppKit.NSMenuItem sender);

		[Action ("saveCipher:")]
		partial void saveCipher (AppKit.NSMenuItem sender);

		[Action ("saveText:")]
		partial void saveText (AppKit.NSMenuItem sender);

		[Action ("temp:")]
		partial void temp (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CipherButton != null) {
				CipherButton.Dispose ();
				CipherButton = null;
			}

			if (CipheredDataTextField != null) {
				CipheredDataTextField.Dispose ();
				CipheredDataTextField = null;
			}

			if (CipheredOutputTextField != null) {
				CipheredOutputTextField.Dispose ();
				CipheredOutputTextField = null;
			}

			if (CipherKeyTextField != null) {
				CipherKeyTextField.Dispose ();
				CipherKeyTextField = null;
			}

			if (DecipherButton != null) {
				DecipherButton.Dispose ();
				DecipherButton = null;
			}

			if (DecipherKeyTextField != null) {
				DecipherKeyTextField.Dispose ();
				DecipherKeyTextField = null;
			}

			if (MainTabView != null) {
				MainTabView.Dispose ();
				MainTabView = null;
			}

			if (RawDataTextField != null) {
				RawDataTextField.Dispose ();
				RawDataTextField = null;
			}

			if (RawOutputTextField != null) {
				RawOutputTextField.Dispose ();
				RawOutputTextField = null;
			}
		}
	}
}
