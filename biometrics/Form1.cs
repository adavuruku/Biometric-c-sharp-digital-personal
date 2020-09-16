using DPUruNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace biometrics
{
    public partial class Form1 : Form
    {
        FingerPrint fp = null;
        Fmd firstFinger = null;
        public Form1()
        {
            InitializeComponent();
            fp = new FingerPrint();
        }
        private ReaderCollection _readers;
        public Reader CurrentReader
        {
            get { return currentReader; }
            set
            {
                currentReader = value;
            }
        }
        private Reader currentReader;

        

        private void Form1_Load(object sender, EventArgs e)
        {
            lblPlaceFinger.Visible = false;
            loadAvaillableReaders();
        }

        //load available reader if multiple reader is selected
        private void loadAvaillableReaders()
        {
            cboReaders.Text = string.Empty;
            cboReaders.Items.Clear();
            cboReaders.SelectedIndex = -1;

            try
            {
                _readers = ReaderCollection.GetReaders();

                foreach (Reader Reader in _readers)
                {
                    cboReaders.Items.Add(Reader.Description.SerialNumber);
                }

                if (cboReaders.Items.Count > 0)
                {
                    cboReaders.SelectedIndex = 0;
                    
                }
                else
                {
                   
                }
            }
            catch (Exception ex)
            {
                //message box:
                String text = ex.Message;
                text += "\r\n\r\nPlease check if DigitalPersona service has been started";
                String caption = "Cannot access readers";
                MessageBox.Show(text, caption);
            }
        }

        //method that runs for every capturing
        public void OnCaptured(CaptureResult captureResult)
        {
            try
            {
                // Check capture quality and throw an error if bad.
                if (!fp.CheckCaptureResult(captureResult))
                {
                    return;
                }else{
                    //convert to a valid finger print first 
                    DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);
                    if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        throw new Exception(resultConversion.ResultCode.ToString());
                       
                    }else{
                        //if it succesfully convert then display for user to see
                        // Create bitmap - this part display the captured on screen
                        foreach (Fid.Fiv fiv in captureResult.Data.Views)
                        {
                            SendMessage(Action.SendBitmap, fp.CreateBitmap(fiv.RawImage, fiv.Width, fiv.Height));
                        }
                        firstFinger = resultConversion.Data;
                        
                    }
                    
                }
                


            }
            catch (Exception ex)
            {
                // Send error message, then close form
                SendMessage(Action.SendMessage, "Error:  " + ex.Message);
            }
        }




        //cancel reading biometrics when processing
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            fp.CancelCaptureAndCloseReader(this.OnCaptured);
        }

        

        //start capturing
        private void btnBack_Click(object sender, EventArgs e)
        {

            if (fp.CurrentReader != null)
            {
                fp.CurrentReader.Dispose();
                fp.CurrentReader = null;
            }
            fp.CurrentReader = _readers[cboReaders.SelectedIndex];

            startReadingFinger();
            
        }

        private void startReadingFinger()
        {
            lblPlaceFinger.Visible = true;
            pbFingerprint.Image = null;

            if (!fp.OpenReader())
            {
                MessageBox.Show("No Reader Selected");
            }

            if (!fp.StartCaptureAsync(this.OnCaptured))
            {
                //this.Close();
            }
        }


        #region SendMessage
        private enum Action
        {
            SendBitmap,
            SendMessage
        }
        private delegate void SendMessageCallback(Action action, object payload);
        private void SendMessage(Action action, object payload)
        {
            try
            {
                if (this.pbFingerprint.InvokeRequired)
                {
                    SendMessageCallback d = new SendMessageCallback(SendMessage);
                    this.Invoke(d, new object[] { action, payload });
                }
                else
                {
                    switch (action)
                    {
                        case Action.SendMessage:
                            MessageBox.Show((string)payload);
                            break;
                        case Action.SendBitmap:
                            lblPlaceFinger.Visible = false;
                            pbFingerprint.Image = (Bitmap)payload;
                            pbFingerprint.Refresh();
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection con = null;
            try
            {
                String ConString = ConfigurationManager.ConnectionStrings["biometrics.Properties.Settings.testreportConnectionString"].ConnectionString;
                con = new MySqlConnection(ConString);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Insert into tblfinger (LedgerId, CustomerFinger) VALUES (@ledger, @fingerPrint)";
                cmd.Parameters.AddWithValue("@ledger", ledgerid.Text.ToString());
                cmd.Parameters.AddWithValue("@fingerPrint", Fmd.SerializeXml(firstFinger));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Created Successfully");
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail To Create User");
                con.Close();
            }
        
        }

        private void login_Click(object sender, EventArgs e)
        {
            new Login().ShowDialog();
        }
    }
}
