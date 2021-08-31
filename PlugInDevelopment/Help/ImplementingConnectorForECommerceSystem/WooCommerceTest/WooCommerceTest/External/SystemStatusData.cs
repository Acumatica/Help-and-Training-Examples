using Newtonsoft.Json;

namespace WooCommerceTest
{
    public class SystemStatusData
    {
        [JsonProperty("environment")]
        public Environment Environment { get; set; }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }
    }

    public class Settings
    {
        [JsonProperty("api_enabled")]
        public bool? ApiEnabled { get; set; }

        [JsonProperty("force_ssl")]
        public bool? ForceSsl { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("currency_position")]
        public string CurrencyPosition { get; set; }

        [JsonProperty("thousand_separator")]
        public string ThousandSeparator { get; set; }

        [JsonProperty("decimal_separator")]
        public string DecimalSeparator { get; set; }

        [JsonProperty("number_of_decimals")]
        public int? NumberOfDecimals { get; set; }

        [JsonProperty("geolocation_enabled")]
        public bool? GeolocationEnabled { get; set; }

        [JsonProperty("woocommerce_com_connected")]
        public string WoocommerceComConnected { get; set; }
    }

    public class Environment
    {
        [JsonProperty("home_url")]
        public string HomeUrl { get; set; }

        [JsonProperty("site_url")]
        public string SiteUrl { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("log_directory")]
        public string LogDirectory { get; set; }

        [JsonProperty("log_directory_writable")]
        public bool? LogDirectoryWritable { get; set; }

        [JsonProperty("wp_version")]
        public string WpVersion { get; set; }

        [JsonProperty("wp_multisite")]
        public bool? WpMultisite { get; set; }

        [JsonProperty("wp_memory_limit")]
        public int? WpMemoryLimit { get; set; }

        [JsonProperty("wp_debug_mode")]
        public bool? WpDebugMode { get; set; }

        [JsonProperty("wp_cron")]
        public bool? WpCron { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("external_object_cache")]
        public object ExternalObjectCache { get; set; }

        [JsonProperty("server_info")]
        public string ServerInfo { get; set; }

        [JsonProperty("php_version")]
        public string PhpVersion { get; set; }

        [JsonProperty("php_post_max_size")]
        public int? PhpPostMaxSize { get; set; }

        [JsonProperty("php_max_execution_time")]
        public int? PhpMaxExecutionTime { get; set; }

        [JsonProperty("php_max_input_vars")]
        public int? PhpMaxInputVars { get; set; }

        [JsonProperty("curl_version")]
        public string CurlVersion { get; set; }

        [JsonProperty("suhosin_installed")]
        public bool? SuhosinInstalled { get; set; }

        [JsonProperty("max_upload_size")]
        public int? MaxUploadSize { get; set; }

        [JsonProperty("mysql_version")]
        public string MysqlVersion { get; set; }

        [JsonProperty("mysql_version_string")]
        public string MysqlVersionString { get; set; }

        [JsonProperty("default_timezone")]
        public string DefaultTimezone { get; set; }

        [JsonProperty("fsockopen_or_curl_enabled")]
        public bool? FsockopenOrCurlEnabled { get; set; }

        [JsonProperty("soapclient_enabled")]
        public bool? SoapclientEnabled { get; set; }

        [JsonProperty("domdocument_enabled")]
        public bool? DomdocumentEnabled { get; set; }

        [JsonProperty("gzip_enabled")]
        public bool? GzipEnabled { get; set; }

        [JsonProperty("mbstring_enabled")]
        public bool? MbstringEnabled { get; set; }

        [JsonProperty("remote_post_successful")]
        public bool? RemotePostSuccessful { get; set; }

        [JsonProperty("remote_post_response")]
        public int RemotePostResponse { get; set; }

        [JsonProperty("remote_get_successful")]
        public bool? RemoteGetSuccessful { get; set; }

        [JsonProperty("remote_get_response")]
        public int? RemoteGetResponse { get; set; }
    }
}
