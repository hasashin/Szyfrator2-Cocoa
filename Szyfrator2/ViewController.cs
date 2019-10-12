using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

using AppKit;
using Foundation;

namespace Szyfrator2
{
    public partial class ViewController : NSViewController
    {
        private readonly ArrayList keyCharsTable = new ArrayList
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
            'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
            'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        private readonly ArrayList messageCharsTable = new ArrayList
        {
            'a','b','c','d','e','f','g','h','i','j','k',
            'l','m','n','o','p','q','r','s','t','u','v',
            'w','x','y','z','A','B','C','D','E','F','G',
            'H','I','J','K','L','M','N','O','P','Q','R',
            'S','T','U','V','W','X','Y','Z','.',' ',',','-',':'
        };

        private readonly Przekatnokolumnowy szyfr;

        private NSAlert CreateAlert(string text, NSAlertStyle alertStyle, string[] buttons)
        {
            NSAlert alert = new NSAlert
            {
                MessageText = text,
                AlertStyle = alertStyle
            };
            foreach(string button in buttons)
            {
                alert.AddButton(button);
            }
            return alert;
        }

        private NSAlert CreateAlert(string text, NSAlertStyle alertStyle)
        {
            NSAlert alert = new NSAlert
            {
                MessageText = text,
                AlertStyle = alertStyle
            };
            return alert;
        }

        private int ShowAlert(string text, NSAlertStyle alertStyle, params string[] buttons)
        {
            var alert = CreateAlert(text, alertStyle, buttons);
            int resp = (int)alert.RunSheetModal(View.Window);
            alert.Dispose();
            return resp;
        }

        private int ShowErrorAlert(string text)
        {
            var alert = CreateAlert(text, NSAlertStyle.Critical);
            int resp = (int)alert.RunSheetModal(View.Window);
            alert.Dispose();
            return resp;
        }

        private bool ShowTrimWarning()
        {
            return 1001 == ShowAlert("W tekście znaleziono nieprawidłowe znaki. Wiadomość zostanie zaszyfrowana bez nich.\nCzy chcesz kontynuować?", NSAlertStyle.Warning,"Nie","Tak");
        }

        private bool TrimWrongChars(NSTextField textField, bool isCipher)
        {
            try
            {
                foreach (char letter in textField.StringValue)
                {
                    if (isCipher)
                    {
                        if (messageCharsTable.Contains(letter) || letter == '#')
                            continue;
                    }
                    else
                    {
                        if (messageCharsTable.Contains(letter))
                            continue;
                    }
                    textField.StringValue = textField.StringValue.Replace(letter.ToString(), "");
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool ValidateText(NSTextField textField, bool isCipher)
        {
            bool warningResponse = true;
            foreach (char letter in textField.StringValue)
            {
                if (messageCharsTable.Contains(letter)) continue;
                if (isCipher && letter == '#') continue;
                warningResponse = ShowTrimWarning();
                break;
            }
            if (warningResponse)
            {
                return TrimWrongChars(textField, isCipher);
            }
            return false;
        }

        private NSSavePanel NewSaveFileDialog(string title, string filter)
        {
            NSSavePanel fileDialog = new NSSavePanel();
            string[] filters = new string[filter.Split(';').Length];
            int x = 0;
            foreach (string filterElem in filter.Split(';'))
            {
                filters[x] = filterElem;
                x++;
            }
            fileDialog.Title = title;
            fileDialog.AllowedFileTypes = filters;
            fileDialog.NameFieldStringValue = filter.Contains("cip")?"szyfr":"";
            return fileDialog;
        }

        private NSOpenPanel NewOpenFileDialog(string title, string filter)
        {
            NSOpenPanel fileDialog = new NSOpenPanel();
            string[] filters = new string[filter.Split(';').Length];
            int x = 0;
            foreach (string filterElem in filter.Split(';'))
            {
                filters[x] = filterElem;
                x++;
            }
            fileDialog.Title = title;
            fileDialog.AllowedFileTypes = filters;
            return fileDialog;
        }

        public ViewController(IntPtr handle) : base(handle)
        {
            szyfr = new Przekatnokolumnowy();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CipherKeyTextField.Changed += KeyTextFieldTextChanged;
            DecipherKeyTextField.Changed += KeyTextFieldTextChanged;
            CipherButton.Activated += OnCipherButtonClicked;
            DecipherButton.Activated += OnDecipherButtonClicked;
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
            }
        }

        protected void KeyTextFieldTextChanged(object sender,EventArgs args)
        {
            NSTextField keyTextField = (sender as NSNotification).Object as NSTextField;
            if(keyTextField.Identifier == "CipherKeyTextField")
            {
                CipherButton.Enabled = keyTextField.StringValue != "";
            }
            else if(keyTextField.Identifier == "DecipherKeyTextField")
            {
                DecipherButton.Enabled = keyTextField.StringValue != "";
            }
            keyTextField.StringValue = keyTextField.StringValue.ToUpper();
            foreach (char letter in keyTextField.StringValue)
            {
                if (!keyCharsTable.Contains(letter))
                {
                    keyTextField.StringValue = keyTextField.StringValue.Replace(letter.ToString(), "");
                }
            }
        }

        protected void OnCipherButtonClicked(object sender, EventArgs e)
        {
            szyfr.Key = CipherKeyTextField.StringValue;
            bool response = false;
            try
            {
                response = ValidateText(RawDataTextField, false);
            }
            catch (Exception ex)
            {
                ShowErrorAlert(ex.Message);
            }
            if (response) CipheredOutputTextField.StringValue = szyfr.Encrypt(RawDataTextField.StringValue);
        }

        protected void OnDecipherButtonClicked(object sender, EventArgs e)
        {
            szyfr.Key = DecipherKeyTextField.StringValue;
            bool response = false;
            try
            {
                response = ValidateText(CipheredDataTextField, true);
            }
            catch (Exception ex)
            {
                ShowErrorAlert(ex.Message);
            }
            if (response) RawOutputTextField.StringValue = szyfr.Decrypt(CipheredDataTextField.StringValue);
        }

        partial void saveCipher(NSMenuItem sender)
        {
            if (CipheredOutputTextField.StringValue == "")
            {
                ShowErrorAlert("Pole wynikowe jest puste.\nWygeneruj nową wiadomość aby móc ją zapisać.");
                return;
            }

            var fileDialog = NewSaveFileDialog("Zapisz plik", "cip");
            if (fileDialog.RunModal() == (int)NSModalResponse.OK)
            {
                StreamWriter cipherFile = new StreamWriter(fileDialog.Url.Path);
                Task dataTask = cipherFile.WriteAsync(CipheredOutputTextField.StringValue);
                dataTask.Wait();
                cipherFile.Close();
            }
            fileDialog.Dispose();
        }

        partial void saveText(NSMenuItem sender)
        {
            if (RawOutputTextField.StringValue == "")
            {
                ShowErrorAlert("Pole wynikowe jest puste.\nOdkoduj kolejną wiadomość aby móc ją zapisać.");
                return;
            }

            var fileDialog = NewSaveFileDialog("Zapisz plik", "txt");
            if (fileDialog.RunModal() == (int)NSModalResponse.OK)
            {
                StreamWriter textFile = new StreamWriter(fileDialog.Url.Path);
                Task dataTask = textFile.WriteAsync(RawOutputTextField.StringValue);
                dataTask.Wait();
                textFile.Close();
            }
            fileDialog.Dispose();
        }

        partial void openCipher(NSMenuItem sender)
        {
            NSOpenPanel fileDialog = NewOpenFileDialog("Wybierz plik do odszyfrowania", "cip");
            if (fileDialog.RunModal() == (int)NSModalResponse.OK)
            {
                StreamReader dataFile = File.OpenText(fileDialog.Url.Path);
                Task<string> dataTask = dataFile.ReadToEndAsync();
                dataTask.Wait();
                CipheredDataTextField.StringValue = dataTask.Result;
                MainTabView.SelectAt(1);
                dataFile.Close();
            }
        }

        partial void openText(NSMenuItem sender)
        {
            NSOpenPanel fileDialog = NewOpenFileDialog("Wybierz plik do zaszyfrowania", "txt");
            if (fileDialog.RunModal() == (int)NSModalResponse.OK)
            {
                StreamReader dataFile = File.OpenText(fileDialog.Url.Path);
                Task<string> dataTask = dataFile.ReadToEndAsync();
                dataTask.Wait();
                RawDataTextField.StringValue = dataTask.Result;
                MainTabView.SelectAt(0);
                dataFile.Close();
            }
        }

        partial void newCipher(NSMenuItem sender)
        {
            int resp = ShowAlert("Wszystkie niezapisane dane zostaną usunięte!\nCzy chcesz kontynuować?", NSAlertStyle.Warning, "Nie","Tak");
            if (resp == 1001)
                CleanAll();
        }

        private void CleanAll()
        {
            RawDataTextField.StringValue = "";
            CipheredDataTextField.StringValue = "";
            CipherKeyTextField.StringValue = "";
            DecipherKeyTextField.StringValue = "";
            CipheredOutputTextField.StringValue = "";
            RawOutputTextField.StringValue = "";
            MainTabView.SelectAt(0);
        }

    }
}
