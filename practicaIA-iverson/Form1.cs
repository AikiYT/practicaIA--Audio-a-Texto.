using System;
using System.Speech.Recognition;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ReconocimientoVoz
{
    public partial class Form1 : Form
    {
        private SpeechRecognitionEngine recognizer;
        private StringBuilder textoReconocido;

        public Form1()
        {
            InitializeComponent();

            // Configurare el motor de reconocimiento de voz
            recognizer = new SpeechRecognitionEngine();
            recognizer.LoadGrammar(new DictationGrammar());
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Recognizer_SpeechRecognized);
            recognizer.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(Recognizer_RecognizeCompleted);

            textoReconocido = new StringBuilder();
        }

        // este es el  Evento de reconocimiento de voz
        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // aqui Agregue el texto reconocido en lugar de sobrescribir
            textoReconocido.Append(e.Result.Text + " ");
            txtResultado.Text = textoReconocido.ToString();
        }

        private void Recognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            MessageBox.Show("El reconocimiento de audio ha finalizado.");
        }


        // Método para encriptar el texto
        private string EncriptarTexto(string texto)
        {
            byte[] data = Encoding.UTF8.GetBytes(texto);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashData = sha256.ComputeHash(data);
                return Convert.ToBase64String(hashData);
            }
        }

        private void btnConvertir_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos WAV|*.wav",
                Title = "Selecciona un archivo de audio"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // esto es para que Limpie el texto reconocido antes de iniciar
                    textoReconocido.Clear();
                    txtResultado.Clear();

                    recognizer.SetInputToWaveFile(openFileDialog.FileName);
                    recognizer.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al procesar el archivo de audio: " + ex.Message);
                }
            }
        }

        private void btnEncriptar_Click(object sender, EventArgs e)
        {
            if (textoReconocido.Length > 0)
            {
                txtResultado.Text = EncriptarTexto(textoReconocido.ToString());
            }
            else
            {
                MessageBox.Show("Primero convierta el audio a texto.");
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
    }


      