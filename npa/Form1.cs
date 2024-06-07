using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace npa
{
    public partial class Form1 : Form
    {
        private readonly string imageDirectory = "images";
        private readonly List<string> imageFiles = new List<string>();
        private int currentImageIndex = 0;
        private readonly Player player = new Player();
        private int imageCounter = 0;
        private readonly Random random = new Random();
        
        public Form1()
        {
            InitializeComponent();
            SetupUI();
            LoadImagesFromDirectory();
        }

        private void SetupUI()
        {
            textBox1.KeyPress += textBox1_KeyPress;
        }

        private void LoadImagesFromDirectory()
        {
            string[] files = Directory.GetFiles(imageDirectory, "*.jpg");
            Shuffle(files);
            imageFiles.AddRange(files);
            LoadNextImage();
            MessageBox.Show("ugani kaj prikazuje slika");
        }

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void LoadNextImage()
        {
            if (currentImageIndex < imageFiles.Count)
            {
                string imagePath = imageFiles[currentImageIndex];
                Image originalImage = Image.FromFile(imagePath);
                int pixelSize = 10;
                Image pixelatedImage = originalImage.GetThumbnailImage(originalImage.Width / pixelSize, originalImage.Height / pixelSize, null, IntPtr.Zero);
                pictureBox1.Image = pixelatedImage.GetThumbnailImage(originalImage.Width, originalImage.Height, null, IntPtr.Zero);
                player.SetCurrentImage(imagePath);
                currentImageIndex++;
                imageCounter++;
            }
            else
            {
                MessageBox.Show($"Vse slike so bile uporabljene. Dosežene toèke: {player.Score} od {imageCounter}");             
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string guess = textBox1.Text.ToLower();
            string correctAnimal = Path.GetFileNameWithoutExtension(player.CurrentImage).ToLower();
            if (guess == correctAnimal)
            {
                player.Score++;
                label1.Text = $"Toèke: {player.Score}";
                MessageBox.Show("Pravilno!");

            }
            else
            {
                MessageBox.Show("Nepravilen odgovor.");
            }
            LoadNextImage();
            textBox1.Text = "";
        }

        private void textBox1_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1.PerformClick();
                e.Handled = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        { }
        private void textBox1_TextChanged(object sender, EventArgs e)
        { }
        private void label1_Click(object sender, EventArgs e)
        { }

    }

    public class Player
    {
        public int Score { get; set; }
        public string CurrentImage { get; private set; }
        public Player()
        {
            Score = 0;
            CurrentImage = "";
        }
        public void SetCurrentImage(string imagePath)
        {
            CurrentImage = imagePath;
        }
    } 
}
