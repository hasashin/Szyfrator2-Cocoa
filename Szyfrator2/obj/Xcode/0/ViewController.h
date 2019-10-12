// WARNING
// This file has been generated automatically by Visual Studio to
// mirror C# types. Changes in this file made by drag-connecting
// from the UI designer will be synchronized back to C#, but
// more complex manual changes may not transfer correctly.


#import <Foundation/Foundation.h>
#import <AppKit/AppKit.h>


@interface ViewController : NSViewController {
	NSButton *_CipherButton;
	NSTextField *_CipheredDataTextField;
	NSTextField *_CipheredOutputTextField;
	NSTextField *_CipherKeyTextField;
	NSButtonCell *_DecipherButton;
	NSTextField *_DecipherKeyTextField;
	NSTabView *_MainTabView;
	NSTextField *_RawDataTextField;
	NSTextField *_RawOutputTextField;
}

@property (nonatomic, retain) IBOutlet NSButton *CipherButton;

@property (nonatomic, retain) IBOutlet NSTextField *CipheredDataTextField;

@property (nonatomic, retain) IBOutlet NSTextField *CipheredOutputTextField;

@property (nonatomic, retain) IBOutlet NSTextField *CipherKeyTextField;

@property (nonatomic, retain) IBOutlet NSButton *DecipherButton;

@property (nonatomic, retain) IBOutlet NSTextField *DecipherKeyTextField;

@property (nonatomic, retain) IBOutlet NSTabView *MainTabView;

@property (nonatomic, retain) IBOutlet NSTextField *RawDataTextField;

@property (nonatomic, retain) IBOutlet NSTextField *RawOutputTextField;

-(IBAction)saveCipher: (NSMenuItem *)sender;

-(IBAction)saveText: (NSMenuItem *)sender;

-(IBAction)openCipher: (NSMenuItem *)sender;

-(IBAction)openText: (NSMenuItem *)sender;

-(IBAction)newCipher: (NSMenuItem *)sender;

-(IBAction)dupadupadupa: (NSMenuItem *)sender;

@end
