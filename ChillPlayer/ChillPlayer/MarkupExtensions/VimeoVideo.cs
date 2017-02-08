using System.Collections.Generic;

namespace ChillPlayer.MarkupExtensions
{
    public class VimeoVideo
    {
        public string cdn_url { get; set; }
        public int view { get; set; }
        public Request request { get; set; }
        public string player_url { get; set; }
        public Video video { get; set; }
        public Build2 build { get; set; }
        public Embed embed { get; set; }
        public string vimeo_url { get; set; }
        public User user { get; set; }
    }

    public class Hls
    {
        public string url { get; set; }
        public string origin { get; set; }
        public string cdn { get; set; }
    }

    public class Progressive
    {
        public int profile { get; set; }
        public int width { get; set; }
        public string mime { get; set; }
        public int fps { get; set; }
        public string url { get; set; }
        public string cdn { get; set; }
        public string quality { get; set; }
        public int id { get; set; }
        public string origin { get; set; }
        public int height { get; set; }
    }

    public class Files
    {
        public Hls hls { get; set; }
        public List<Progressive> progressive { get; set; }
    }

    public class Cookie
    {
        public int scaling { get; set; }
        public double volume { get; set; }
        public object quality { get; set; }
        public int hd { get; set; }
        public object captions { get; set; }
    }

    public class Flags
    {
        public int dnt { get; set; }
        public string preload_video { get; set; }
        public int plays { get; set; }
        public int webp { get; set; }
        public int conviva { get; set; }
        public int flash_hls { get; set; }
        public int login { get; set; }
        public int partials { get; set; }
        public int blurr { get; set; }
    }

    public class Build
    {
        public string player { get; set; }
        public string js { get; set; }
    }

    public class Urls
    {
        public string zeroclip_swf { get; set; }
        public string js { get; set; }
        public string proxy { get; set; }
        public string flideo { get; set; }
        public string moog { get; set; }
        public string comscore_js { get; set; }
        public string blurr { get; set; }
        public string vuid_js { get; set; }
        public string moog_js { get; set; }
        public string zeroclip_js { get; set; }
        public string css { get; set; }
    }

    public class Request
    {
        public Files files { get; set; }
        public string ga_account { get; set; }
        public int expires { get; set; }
        public int timestamp { get; set; }
        public string signature { get; set; }
        public string currency { get; set; }
        public string session { get; set; }
        public Cookie cookie { get; set; }
        public string cookie_domain { get; set; }
        public object referrer { get; set; }
        public string comscore_id { get; set; }
        public Flags flags { get; set; }
        public Build build { get; set; }
        public Urls urls { get; set; }
        public string country { get; set; }
    }

    public class Rating
    {
        public int id { get; set; }
    }

    public class Owner
    {
        public string account_type { get; set; }
        public string name { get; set; }
        public string img { get; set; }
        public string url { get; set; }
        public string img_2x { get; set; }
        public int id { get; set; }
    }

    public class Thumbs
    {
        public string __invalid_name__1280 { get; set; }
        public string __invalid_name__960 { get; set; }
        public string __invalid_name__640 { get; set; }
        public string @base { get; set; }
    }

    public class Video
    {
        public Rating rating { get; set; }
        public int allow_hd { get; set; }
        public int height { get; set; }
        public Owner owner { get; set; }
        public Thumbs thumbs { get; set; }
        public int duration { get; set; }
        public int id { get; set; }
        public int hd { get; set; }
        public string embed_code { get; set; }
        public int default_to_hd { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string privacy { get; set; }
        public string share_url { get; set; }
        public int width { get; set; }
        public string embed_permission { get; set; }
        public double fps { get; set; }
    }

    public class Build2
    {
        public string player { get; set; }
        public string rpc { get; set; }
    }

    public class Settings
    {
        public int fullscreen { get; set; }
        public int byline { get; set; }
        public int like { get; set; }
        public int playbar { get; set; }
        public int title { get; set; }
        public int color { get; set; }
        public int branding { get; set; }
        public int share { get; set; }
        public int scaling { get; set; }
        public int logo { get; set; }
        public int collections { get; set; }
        public int info_on_pause { get; set; }
        public int watch_later { get; set; }
        public int portrait { get; set; }
        public int embed { get; set; }
        public int volume { get; set; }
    }

    public class Embed
    {
        public string player_id { get; set; }
        public string outro { get; set; }
        public int api { get; set; }
        public string context { get; set; }
        public int time { get; set; }
        public string color { get; set; }
        public Settings settings { get; set; }
        public int on_site { get; set; }
        public int loop { get; set; }
        public int autoplay { get; set; }
    }

    public class User
    {
        public int liked { get; set; }
        public string account_type { get; set; }
        public int progress { get; set; }
        public int owner { get; set; }
        public int watch_later { get; set; }
        public int logged_in { get; set; }
        public int id { get; set; }
        public int mod { get; set; }
    }
}
