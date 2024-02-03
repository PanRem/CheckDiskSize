using System;
using System.IO;

namespace testQuota
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
			//on lance le timer au lancement de l'app
			//de base le form a une oppacity de 0 et est donc invisible pour l'utilisateur
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
			//lors du clic sur ok on effectue la meme action qu'au clic sur la croix de fermeture
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
			//on défini ici nos variable ainsi que la lettre du lecteur a vérifier
            string nom = "J:\\";
            double tailleFull = 0;
            double tailleFree = 0;
			//on récupère les infos des lecteurs
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    if (drive.DriveType == DriveType.Network && drive.Name == nom)
                    {
						//on enregistre la taille dispo et la taille max du lecteur qui nous intéresse (ici J)
                        tailleFree = drive.AvailableFreeSpace;
                        tailleFull = drive.TotalSize;
                    }
                }
            }
			//on check si la taille est correct 
            if (tailleFull > 0)
            {
				//pourcentage de taille libre
                double percent = (tailleFree / tailleFull) * 100;
                if (percent < 10)
                {
					//si moins de 10% on affiche le message avec la taille restante,
					//on passe également le timer a 4h pour laisser le temps a l'utilisateur de modifier
                    size.Text = percent.ToString("0.0") + "% (" + (tailleFree / 1000000).ToString("0.00") + " mo)";
                    this.Opacity = 100;
                    timer1.Interval = 14400000;
                }
                else
                {
					//si on est a plus de 10% on met le timer afin de vérifier toutes les minutes
                    timer1.Interval = 60000;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
			//dans le cas de la fermeture de la form on vérifie si c'est initié par l'utilisateur
			//si oui on annule la fermeture mais on cache la fenetre pour que le timer tourne en fond
            if (e.CloseReason.Equals(CloseReason.UserClosing))
            {
                e.Cancel = true;
                this.Opacity = 0;
            }
        }
    }
}