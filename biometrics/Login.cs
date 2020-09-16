using DPUruNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace biometrics
{
    public partial class Login : Form
    {
        FingerPrint fp = null;
        Fmd firstFinger = null;
        private const int PROBABILITY_ONE = 0x7fffffff;
        public Login()
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
        private void Login_Load(object sender, EventArgs e)
        {
            lblPlaceFinger.Visible = false;
            loadAvaillableReaders();

            if (fp.CurrentReader != null)
            {
                fp.CurrentReader.Dispose();
                fp.CurrentReader = null;
            }
            fp.CurrentReader = _readers[cboReaders.SelectedIndex];

            startReadingFinger();
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
                }
                else
                {
                    //convert to a valid finger print first 
                    DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);
                    if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        throw new Exception(resultConversion.ResultCode.ToString());

                    }
                    else
                    {
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

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            fp.CancelCaptureAndCloseReader(this.OnCaptured);
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection con = null;
            Boolean got = false;
            try
            {
                String ConString = ConfigurationManager.ConnectionStrings["biometrics.Properties.Settings.testreportConnectionString"].ConnectionString;
                con = new MySqlConnection(ConString);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                
                String query = "Select * from tblfinger";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Prepare();
                MySqlDataReader row = cmd.ExecuteReader();
                if (row.HasRows)
                {
                    while (row.Read())
                    {
                        Fmd previousFinger = Fmd.DeserializeXml(row["CustomerFinger"].ToString());
                        
                        CompareResult compareResult = Comparison.Compare(firstFinger, 0, previousFinger, 0);
                        if (compareResult.ResultCode == Constants.ResultCode.DP_SUCCESS)
                        {
                            if((compareResult.Score < (PROBABILITY_ONE / 100000))){
                                MessageBox.Show("Welcome TO Our Page " + row["LedgerId"].ToString());
                                got = true;
                                break;
                            }
                        }
                    }
                }
                con.Close();

                if (!got)
                {
                    lblPlaceFinger.Visible = true;
                    pbFingerprint.Image = null;
                    MessageBox.Show("Invalid Information. Please Retry ");
                    firstFinger = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail To Create User - " + ex.Message);
                con.Close();
            }
        }
    }
}
