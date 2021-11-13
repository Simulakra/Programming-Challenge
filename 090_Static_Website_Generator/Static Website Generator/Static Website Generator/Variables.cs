using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Static_Website_Generator
{
    public class Variables
    {
        public enum Themes { Dark, Light }
        public enum SocialMedias
        {
            Blogger, Discord, Facebook, Git,
            GitHub, GitLab, Instagram, LinkedIn,
            Pinterest, Reddit, Spotify, Tumblr,
            Twitch, Twitter, Wordpress, Youtube,
        }

        public static Dictionary<SocialMedias, string> SocialMediaIcons= new Dictionary<SocialMedias, string>()
        {
            { SocialMedias.Blogger, "fa-blogger" },
            { SocialMedias.Discord, "fa-discord" },
            { SocialMedias.Facebook,  "fa-facebook-f" },
            { SocialMedias.Git, "fa-git-alt" },
            { SocialMedias.GitHub, "fa-github" },
            { SocialMedias.GitLab, "fa-gitlab" },
            { SocialMedias.Instagram,  "fa-instagram" },
            { SocialMedias.LinkedIn,  "fa-linkedin-in" },
            { SocialMedias.Pinterest, "fa-pinterest" },
            { SocialMedias.Reddit, "fa-reddit-alien" },
            { SocialMedias.Spotify, "fa-spotify" },
            { SocialMedias.Tumblr, "fa-tumblr" },
            { SocialMedias.Twitch, "fa-twitch" },
            { SocialMedias.Twitter,  "fa-twitch" },
            { SocialMedias.Wordpress, "fa-wordpress" },
            { SocialMedias.Youtube, "fa-youtube" },
        };

        public Themes Theme;
        public string SiteName;
        public string PageTitle_Home;
        public string PageTitle_About;
        public string PageTitle_Resume;
        public string PageTitle_Portfolio;
        public string PageTitle_Contact;
        public List<string> Titles;
        public List<string[]> Socials;
        public string DownloadButton;
        public string DownloadLink;
        public string DownloadFileName;

        //Contact
        public string Location;
        public string Phone;
        public string Email;
        public string Freelance;
        public string Map_Location;
    }
}