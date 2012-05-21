-- phpMyAdmin SQL Dump
-- version 3.2.4
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Mar 01, 2012 at 11:44 AM
-- Server version: 5.1.41
-- PHP Version: 5.3.1

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";



--
-- Database: `wordpress`
--

-- --------------------------------------------------------

--
-- Table structure for table `sil_multilingual_search`
--

CREATE TABLE IF NOT EXISTS `sil_multilingual_search` (
  `post_id` bigint(20) NOT NULL,
  `language_code` varchar(20) NOT NULL,
  `relevance` tinyint(4) NOT NULL DEFAULT '0',
  `search_strings` longtext CHARACTER SET utf8,
  PRIMARY KEY (`post_id`,`language_code`,`relevance`),
  KEY `relevance` (`relevance`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `sil_multilingual_search`
--


-- --------------------------------------------------------

--
-- Table structure for table `wp_commentmeta`
--

CREATE TABLE IF NOT EXISTS `wp_commentmeta` (
  `meta_id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `comment_id` bigint(20) unsigned NOT NULL DEFAULT '0',
  `meta_key` varchar(255) DEFAULT NULL,
  `meta_value` longtext,
  PRIMARY KEY (`meta_id`),
  KEY `comment_id` (`comment_id`),
  KEY `meta_key` (`meta_key`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

--
-- Dumping data for table `wp_commentmeta`
--


-- --------------------------------------------------------

--
-- Table structure for table `wp_comments`
--

CREATE TABLE IF NOT EXISTS `wp_comments` (
  `comment_ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `comment_post_ID` bigint(20) unsigned NOT NULL DEFAULT '0',
  `comment_author` tinytext NOT NULL,
  `comment_author_email` varchar(100) NOT NULL DEFAULT '',
  `comment_author_url` varchar(200) NOT NULL DEFAULT '',
  `comment_author_IP` varchar(100) NOT NULL DEFAULT '',
  `comment_date` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `comment_date_gmt` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `comment_content` text NOT NULL,
  `comment_karma` int(11) NOT NULL DEFAULT '0',
  `comment_approved` varchar(20) NOT NULL DEFAULT '1',
  `comment_agent` varchar(255) NOT NULL DEFAULT '',
  `comment_type` varchar(20) NOT NULL DEFAULT '',
  `comment_parent` bigint(20) unsigned NOT NULL DEFAULT '0',
  `user_id` bigint(20) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`comment_ID`),
  KEY `comment_approved` (`comment_approved`),
  KEY `comment_post_ID` (`comment_post_ID`),
  KEY `comment_approved_date_gmt` (`comment_approved`,`comment_date_gmt`),
  KEY `comment_date_gmt` (`comment_date_gmt`),
  KEY `comment_parent` (`comment_parent`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `wp_comments`
--

INSERT INTO `wp_comments` (`comment_ID`, `comment_post_ID`, `comment_author`, `comment_author_email`, `comment_author_url`, `comment_author_IP`, `comment_date`, `comment_date_gmt`, `comment_content`, `comment_karma`, `comment_approved`, `comment_agent`, `comment_type`, `comment_parent`, `user_id`) VALUES
(1, 1, 'Mr WordPress', '', 'http://wordpress.org/', '', '2012-02-28 03:06:04', '2012-02-28 03:06:04', 'Hi, this is a comment.<br />To delete a comment, just log in and view the post&#039;s comments. There you will have the option to edit or delete them.', 0, 'post-trashed', '', '', 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `wp_links`
--

CREATE TABLE IF NOT EXISTS `wp_links` (
  `link_id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `link_url` varchar(255) NOT NULL DEFAULT '',
  `link_name` varchar(255) NOT NULL DEFAULT '',
  `link_image` varchar(255) NOT NULL DEFAULT '',
  `link_target` varchar(25) NOT NULL DEFAULT '',
  `link_description` varchar(255) NOT NULL DEFAULT '',
  `link_visible` varchar(20) NOT NULL DEFAULT 'Y',
  `link_owner` bigint(20) unsigned NOT NULL DEFAULT '1',
  `link_rating` int(11) NOT NULL DEFAULT '0',
  `link_updated` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `link_rel` varchar(255) NOT NULL DEFAULT '',
  `link_notes` mediumtext NOT NULL,
  `link_rss` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`link_id`),
  KEY `link_visible` (`link_visible`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=8 ;

--
-- Dumping data for table `wp_links`
--

INSERT INTO `wp_links` (`link_id`, `link_url`, `link_name`, `link_image`, `link_target`, `link_description`, `link_visible`, `link_owner`, `link_rating`, `link_updated`, `link_rel`, `link_notes`, `link_rss`) VALUES
(1, 'http://codex.wordpress.org/', 'Documentation', '', '', '', 'Y', 1, 0, '0000-00-00 00:00:00', '', '', ''),
(2, 'http://wordpress.org/news/', 'WordPress Blog', '', '', '', 'Y', 1, 0, '0000-00-00 00:00:00', '', '', 'http://wordpress.org/news/feed/'),
(3, 'http://wordpress.org/extend/ideas/', 'Suggest Ideas', '', '', '', 'Y', 1, 0, '0000-00-00 00:00:00', '', '', ''),
(4, 'http://wordpress.org/support/', 'Support Forum', '', '', '', 'Y', 1, 0, '0000-00-00 00:00:00', '', '', ''),
(5, 'http://wordpress.org/extend/plugins/', 'Plugins', '', '', '', 'Y', 1, 0, '0000-00-00 00:00:00', '', '', ''),
(6, 'http://wordpress.org/extend/themes/', 'Themes', '', '', '', 'Y', 1, 0, '0000-00-00 00:00:00', '', '', ''),
(7, 'http://planet.wordpress.org/', 'WordPress Planet', '', '', '', 'Y', 1, 0, '0000-00-00 00:00:00', '', '', '');

-- --------------------------------------------------------

--
-- Table structure for table `wp_options`
--

CREATE TABLE IF NOT EXISTS `wp_options` (
  `option_id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `blog_id` int(11) NOT NULL DEFAULT '0',
  `option_name` varchar(64) NOT NULL DEFAULT '',
  `option_value` longtext NOT NULL,
  `autoload` varchar(20) NOT NULL DEFAULT 'yes',
  PRIMARY KEY (`option_id`),
  UNIQUE KEY `option_name` (`option_name`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=196 ;

--
-- Dumping data for table `wp_options`
--

INSERT INTO `wp_options` (`option_id`, `blog_id`, `option_name`, `option_value`, `autoload`) VALUES
(194, 0, '_site_transient_timeout_theme_roots', '1330584251', 'yes'),
(195, 0, '_site_transient_theme_roots', 'a:4:{s:23:"Multilingual-Dictionary";s:7:"/themes";s:12:"twentyeleven";s:7:"/themes";s:9:"twentyten";s:7:"/themes";s:19:"webonary-zeedisplay";s:7:"/themes";}', 'yes'),
(3, 0, 'siteurl', 'http://localhost/wordpress', 'yes'),
(4, 0, 'blogname', 'Webonary', 'yes'),
(5, 0, 'blogdescription', 'Online searchable dictionary', 'yes'),
(6, 0, 'users_can_register', '0', 'yes'),
(7, 0, 'admin_email', 'your_name@yahoo.com', 'yes'),
(8, 0, 'start_of_week', '1', 'yes'),
(9, 0, 'use_balanceTags', '0', 'yes'),
(10, 0, 'use_smilies', '1', 'yes'),
(11, 0, 'require_name_email', '1', 'yes'),
(12, 0, 'comments_notify', '1', 'yes'),
(13, 0, 'posts_per_rss', '10', 'yes'),
(14, 0, 'rss_use_excerpt', '0', 'yes'),
(15, 0, 'mailserver_url', 'mail.example.com', 'yes'),
(16, 0, 'mailserver_login', 'login@example.com', 'yes'),
(17, 0, 'mailserver_pass', 'password', 'yes'),
(18, 0, 'mailserver_port', '110', 'yes'),
(19, 0, 'default_category', '1', 'yes'),
(20, 0, 'default_comment_status', 'open', 'yes'),
(21, 0, 'default_ping_status', 'open', 'yes'),
(22, 0, 'default_pingback_flag', '1', 'yes'),
(23, 0, 'default_post_edit_rows', '20', 'yes'),
(24, 0, 'posts_per_page', '10', 'yes'),
(25, 0, 'date_format', 'F j, Y', 'yes'),
(26, 0, 'time_format', 'g:i a', 'yes'),
(27, 0, 'links_updated_date_format', 'F j, Y g:i a', 'yes'),
(28, 0, 'links_recently_updated_prepend', '<em>', 'yes'),
(29, 0, 'links_recently_updated_append', '</em>', 'yes'),
(30, 0, 'links_recently_updated_time', '120', 'yes'),
(31, 0, 'comment_moderation', '0', 'yes'),
(32, 0, 'moderation_notify', '1', 'yes'),
(33, 0, 'permalink_structure', '', 'yes'),
(34, 0, 'gzipcompression', '0', 'yes'),
(35, 0, 'hack_file', '0', 'yes'),
(36, 0, 'blog_charset', 'UTF-8', 'yes'),
(37, 0, 'moderation_keys', '', 'no'),
(38, 0, 'active_plugins', 'a:6:{i:0;s:59:"block-spam-by-math-reloaded/block-spam-by-math-reloaded.php";i:1;s:69:"degradable-html5-audio-and-video/degradable-html5-audio-and-video.php";i:2;s:25:"qtranslate/qtranslate.php";i:3;s:37:"share-and-follow/share-and-follow.php";i:4;s:33:"sil-dictionary/sil-dictionary.php";i:5;s:41:"wordpress-importer/wordpress-importer.php";}', 'yes'),
(39, 0, 'home', 'http://localhost/wordpress', 'yes'),
(40, 0, 'category_base', '', 'yes'),
(41, 0, 'ping_sites', 'http://rpc.pingomatic.com/', 'yes'),
(42, 0, 'advanced_edit', '0', 'yes'),
(43, 0, 'comment_max_links', '2', 'yes'),
(44, 0, 'gmt_offset', '6', 'yes'),
(45, 0, 'default_email_category', '1', 'yes'),
(46, 0, 'recently_edited', 'a:3:{i:0;s:82:"C:\\xampp\\htdocs\\wordpress/wp-content/plugins/share-and-follow/share-and-follow.php";i:1;s:73:"C:\\xampp\\htdocs\\wordpress/wp-content/themes/webonary-zeedisplay/style.css";i:2;s:0:"";}', 'no'),
(47, 0, 'template', 'webonary-zeedisplay', 'yes'),
(48, 0, 'stylesheet', 'webonary-zeedisplay', 'yes'),
(49, 0, 'comment_whitelist', '1', 'yes'),
(50, 0, 'blacklist_keys', '', 'no'),
(51, 0, 'comment_registration', '0', 'yes'),
(52, 0, 'rss_language', 'en', 'yes'),
(53, 0, 'html_type', 'text/html', 'yes'),
(54, 0, 'use_trackback', '0', 'yes'),
(55, 0, 'default_role', 'subscriber', 'yes'),
(56, 0, 'db_version', '19470', 'yes'),
(57, 0, 'uploads_use_yearmonth_folders', '1', 'yes'),
(58, 0, 'upload_path', '', 'yes'),
(59, 0, 'blog_public', '1', 'yes'),
(60, 0, 'default_link_category', '2', 'yes'),
(61, 0, 'show_on_front', 'page', 'yes'),
(62, 0, 'tag_base', '', 'yes'),
(63, 0, 'show_avatars', '1', 'yes'),
(64, 0, 'avatar_rating', 'G', 'yes'),
(65, 0, 'upload_url_path', '', 'yes'),
(66, 0, 'thumbnail_size_w', '150', 'yes'),
(67, 0, 'thumbnail_size_h', '150', 'yes'),
(68, 0, 'thumbnail_crop', '1', 'yes'),
(69, 0, 'medium_size_w', '300', 'yes'),
(70, 0, 'medium_size_h', '300', 'yes'),
(71, 0, 'avatar_default', 'mystery', 'yes'),
(72, 0, 'enable_app', '0', 'yes'),
(73, 0, 'enable_xmlrpc', '0', 'yes'),
(74, 0, 'large_size_w', '1024', 'yes'),
(75, 0, 'large_size_h', '1024', 'yes'),
(76, 0, 'image_default_link_type', 'file', 'yes'),
(77, 0, 'image_default_size', '', 'yes'),
(78, 0, 'image_default_align', '', 'yes'),
(79, 0, 'close_comments_for_old_posts', '0', 'yes'),
(80, 0, 'close_comments_days_old', '14', 'yes'),
(81, 0, 'thread_comments', '1', 'yes'),
(82, 0, 'thread_comments_depth', '5', 'yes'),
(83, 0, 'page_comments', '0', 'yes'),
(84, 0, 'comments_per_page', '50', 'yes'),
(85, 0, 'default_comments_page', 'newest', 'yes'),
(86, 0, 'comment_order', 'asc', 'yes'),
(87, 0, 'sticky_posts', 'a:0:{}', 'yes'),
(88, 0, 'widget_categories', 'a:2:{i:2;a:4:{s:5:"title";s:5:"Index";s:5:"count";i:1;s:12:"hierarchical";i:1;s:8:"dropdown";i:1;}s:12:"_multiwidget";i:1;}', 'yes'),
(89, 0, 'widget_text', 'a:0:{}', 'yes'),
(90, 0, 'widget_rss', 'a:0:{}', 'yes'),
(91, 0, 'timezone_string', '', 'yes'),
(92, 0, 'embed_autourls', '1', 'yes'),
(93, 0, 'embed_size_w', '', 'yes'),
(94, 0, 'embed_size_h', '600', 'yes'),
(95, 0, 'page_for_posts', '0', 'yes'),
(96, 0, 'page_on_front', '2', 'yes'),
(97, 0, 'default_post_format', '0', 'yes'),
(98, 0, 'initial_db_version', '19470', 'yes'),
(99, 0, 'wp_user_roles', 'a:5:{s:13:"administrator";a:2:{s:4:"name";s:13:"Administrator";s:12:"capabilities";a:62:{s:13:"switch_themes";b:1;s:11:"edit_themes";b:1;s:16:"activate_plugins";b:1;s:12:"edit_plugins";b:1;s:10:"edit_users";b:1;s:10:"edit_files";b:1;s:14:"manage_options";b:1;s:17:"moderate_comments";b:1;s:17:"manage_categories";b:1;s:12:"manage_links";b:1;s:12:"upload_files";b:1;s:6:"import";b:1;s:15:"unfiltered_html";b:1;s:10:"edit_posts";b:1;s:17:"edit_others_posts";b:1;s:20:"edit_published_posts";b:1;s:13:"publish_posts";b:1;s:10:"edit_pages";b:1;s:4:"read";b:1;s:8:"level_10";b:1;s:7:"level_9";b:1;s:7:"level_8";b:1;s:7:"level_7";b:1;s:7:"level_6";b:1;s:7:"level_5";b:1;s:7:"level_4";b:1;s:7:"level_3";b:1;s:7:"level_2";b:1;s:7:"level_1";b:1;s:7:"level_0";b:1;s:17:"edit_others_pages";b:1;s:20:"edit_published_pages";b:1;s:13:"publish_pages";b:1;s:12:"delete_pages";b:1;s:19:"delete_others_pages";b:1;s:22:"delete_published_pages";b:1;s:12:"delete_posts";b:1;s:19:"delete_others_posts";b:1;s:22:"delete_published_posts";b:1;s:20:"delete_private_posts";b:1;s:18:"edit_private_posts";b:1;s:18:"read_private_posts";b:1;s:20:"delete_private_pages";b:1;s:18:"edit_private_pages";b:1;s:18:"read_private_pages";b:1;s:12:"delete_users";b:1;s:12:"create_users";b:1;s:17:"unfiltered_upload";b:1;s:14:"edit_dashboard";b:1;s:14:"update_plugins";b:1;s:14:"delete_plugins";b:1;s:15:"install_plugins";b:1;s:13:"update_themes";b:1;s:14:"install_themes";b:1;s:11:"update_core";b:1;s:10:"list_users";b:1;s:12:"remove_users";b:1;s:9:"add_users";b:1;s:13:"promote_users";b:1;s:18:"edit_theme_options";b:1;s:13:"delete_themes";b:1;s:6:"export";b:1;}}s:6:"editor";a:2:{s:4:"name";s:6:"Editor";s:12:"capabilities";a:34:{s:17:"moderate_comments";b:1;s:17:"manage_categories";b:1;s:12:"manage_links";b:1;s:12:"upload_files";b:1;s:15:"unfiltered_html";b:1;s:10:"edit_posts";b:1;s:17:"edit_others_posts";b:1;s:20:"edit_published_posts";b:1;s:13:"publish_posts";b:1;s:10:"edit_pages";b:1;s:4:"read";b:1;s:7:"level_7";b:1;s:7:"level_6";b:1;s:7:"level_5";b:1;s:7:"level_4";b:1;s:7:"level_3";b:1;s:7:"level_2";b:1;s:7:"level_1";b:1;s:7:"level_0";b:1;s:17:"edit_others_pages";b:1;s:20:"edit_published_pages";b:1;s:13:"publish_pages";b:1;s:12:"delete_pages";b:1;s:19:"delete_others_pages";b:1;s:22:"delete_published_pages";b:1;s:12:"delete_posts";b:1;s:19:"delete_others_posts";b:1;s:22:"delete_published_posts";b:1;s:20:"delete_private_posts";b:1;s:18:"edit_private_posts";b:1;s:18:"read_private_posts";b:1;s:20:"delete_private_pages";b:1;s:18:"edit_private_pages";b:1;s:18:"read_private_pages";b:1;}}s:6:"author";a:2:{s:4:"name";s:6:"Author";s:12:"capabilities";a:10:{s:12:"upload_files";b:1;s:10:"edit_posts";b:1;s:20:"edit_published_posts";b:1;s:13:"publish_posts";b:1;s:4:"read";b:1;s:7:"level_2";b:1;s:7:"level_1";b:1;s:7:"level_0";b:1;s:12:"delete_posts";b:1;s:22:"delete_published_posts";b:1;}}s:11:"contributor";a:2:{s:4:"name";s:11:"Contributor";s:12:"capabilities";a:5:{s:10:"edit_posts";b:1;s:4:"read";b:1;s:7:"level_1";b:1;s:7:"level_0";b:1;s:12:"delete_posts";b:1;}}s:10:"subscriber";a:2:{s:4:"name";s:10:"Subscriber";s:12:"capabilities";a:2:{s:4:"read";b:1;s:7:"level_0";b:1;}}}', 'yes'),
(100, 0, 'widget_search', 'a:2:{i:2;a:1:{s:5:"title";s:0:"";}s:12:"_multiwidget";i:1;}', 'yes'),
(101, 0, 'widget_recent-posts', 'a:2:{i:2;a:2:{s:5:"title";s:0:"";s:6:"number";i:5;}s:12:"_multiwidget";i:1;}', 'yes'),
(102, 0, 'widget_recent-comments', 'a:2:{i:2;a:2:{s:5:"title";s:0:"";s:6:"number";i:5;}s:12:"_multiwidget";i:1;}', 'yes'),
(103, 0, 'widget_archives', 'a:2:{i:2;a:3:{s:5:"title";s:0:"";s:5:"count";i:0;s:8:"dropdown";i:0;}s:12:"_multiwidget";i:1;}', 'yes'),
(104, 0, 'widget_meta', 'a:2:{i:2;a:1:{s:5:"title";s:0:"";}s:12:"_multiwidget";i:1;}', 'yes'),
(105, 0, 'sidebars_widgets', 'a:5:{s:19:"wp_inactive_widgets";a:3:{i:0;s:6:"meta-2";i:1;s:10:"archives-2";i:2;s:14:"recent-posts-2";}s:12:"sidebar-blog";a:4:{i:0;s:8:"search-2";i:1;s:17:"recent-comments-2";i:2;s:12:"categories-2";i:3;s:11:"tag_cloud-2";}s:13:"sidebar-pages";a:0:{}s:14:"sidebar-footer";a:0:{}s:13:"array_version";i:3;}', 'yes'),
(106, 0, 'cron', 'a:4:{i:1330577893;a:1:{s:12:"qs_cron_hook";a:1:{s:32:"40cd750bba9870f18aada2478b24840a";a:3:{s:8:"schedule";s:6:"hourly";s:4:"args";a:0:{}s:8:"interval";i:3600;}}}i:1330614379;a:3:{s:16:"wp_version_check";a:1:{s:32:"40cd750bba9870f18aada2478b24840a";a:3:{s:8:"schedule";s:10:"twicedaily";s:4:"args";a:0:{}s:8:"interval";i:43200;}}s:17:"wp_update_plugins";a:1:{s:32:"40cd750bba9870f18aada2478b24840a";a:3:{s:8:"schedule";s:10:"twicedaily";s:4:"args";a:0:{}s:8:"interval";i:43200;}}s:16:"wp_update_themes";a:1:{s:32:"40cd750bba9870f18aada2478b24840a";a:3:{s:8:"schedule";s:10:"twicedaily";s:4:"args";a:0:{}s:8:"interval";i:43200;}}}i:1330657614;a:1:{s:19:"wp_scheduled_delete";a:1:{s:32:"40cd750bba9870f18aada2478b24840a";a:3:{s:8:"schedule";s:5:"daily";s:4:"args";a:0:{}s:8:"interval";i:86400;}}}s:7:"version";i:2;}', 'yes'),
(108, 0, '_site_transient_update_core', 'O:8:"stdClass":3:{s:7:"updates";a:0:{}s:15:"version_checked";s:5:"3.3.1";s:12:"last_checked";i:1330577051;}', 'yes'),
(109, 0, '_site_transient_update_plugins', 'O:8:"stdClass":1:{s:12:"last_checked";i:1330577051;}', 'yes'),
(110, 0, '_site_transient_update_themes', 'O:8:"stdClass":1:{s:12:"last_checked";i:1330577051;}', 'yes'),
(111, 0, 'dashboard_widget_options', 'a:4:{s:25:"dashboard_recent_comments";a:1:{s:5:"items";i:5;}s:24:"dashboard_incoming_links";a:5:{s:4:"home";s:26:"http://localhost/wordpress";s:4:"link";s:102:"http://blogsearch.google.com/blogsearch?scoring=d&partner=wordpress&q=link:http://localhost/wordpress/";s:3:"url";s:135:"http://blogsearch.google.com/blogsearch_feeds?scoring=d&ie=utf-8&num=10&output=rss&partner=wordpress&q=link:http://localhost/wordpress/";s:5:"items";i:10;s:9:"show_date";b:0;}s:17:"dashboard_primary";a:7:{s:4:"link";s:26:"http://wordpress.org/news/";s:3:"url";s:31:"http://wordpress.org/news/feed/";s:5:"title";s:14:"WordPress Blog";s:5:"items";i:2;s:12:"show_summary";i:1;s:11:"show_author";i:0;s:9:"show_date";i:1;}s:19:"dashboard_secondary";a:7:{s:4:"link";s:28:"http://planet.wordpress.org/";s:3:"url";s:33:"http://planet.wordpress.org/feed/";s:5:"title";s:20:"Other WordPress News";s:5:"items";i:5;s:12:"show_summary";i:0;s:11:"show_author";i:0;s:9:"show_date";i:0;}}', 'yes'),
(138, 0, 'current_theme', 'webonary-zeedisplay', 'yes'),
(176, 0, '_transient_dash_4077549d03da2e451c8b5f002294ff51', '<div class="rss-widget"><p><strong>RSS Error</strong>: WP HTTP Error: Could not open handle for fopen() to http://wordpress.org/news/feed/</p></div>', 'no'),
(177, 0, '_transient_timeout_dash_aa95765b5cc111c56d5993d476b1c2f0', '1330595171', 'no'),
(178, 0, '_transient_dash_aa95765b5cc111c56d5993d476b1c2f0', '<div class="rss-widget"><p><strong>RSS Error</strong>: WP HTTP Error: Could not open handle for fopen() to http://planet.wordpress.org/feed/</p></div>', 'no'),
(182, 0, '_transient_dash_de3249c4736ad3bd2cd29147c4a0d43e', '', 'no'),
(175, 0, '_transient_timeout_dash_4077549d03da2e451c8b5f002294ff51', '1330595171', 'no'),
(120, 0, 'can_compress_scripts', '1', 'yes'),
(179, 0, '_transient_timeout_plugin_slugs', '1330638862', 'no'),
(180, 0, '_transient_plugin_slugs', 'a:8:{i:0;s:19:"akismet/akismet.php";i:1;s:59:"block-spam-by-math-reloaded/block-spam-by-math-reloaded.php";i:2;s:69:"degradable-html5-audio-and-video/degradable-html5-audio-and-video.php";i:3;s:9:"hello.php";i:4;s:25:"qtranslate/qtranslate.php";i:5;s:37:"share-and-follow/share-and-follow.php";i:6;s:33:"sil-dictionary/sil-dictionary.php";i:7;s:41:"wordpress-importer/wordpress-importer.php";}', 'no'),
(181, 0, '_transient_timeout_dash_de3249c4736ad3bd2cd29147c4a0d43e', '1330595172', 'no'),
(127, 0, '_transient_random_seed', '5100e7117efd7c0c276fba143a40e912', 'yes'),
(128, 0, 'recently_activated', 'a:0:{}', 'yes'),
(129, 0, 'uninstall_plugins', 'a:3:{i:0;b:0;s:59:"block-spam-by-math-reloaded/block-spam-by-math-reloaded.php";s:14:"bsbm_uninstall";s:33:"sil-dictionary/sil-dictionary.php";s:39:"uninstall_sil_dictionary_infrastructure";}', 'yes'),
(130, 0, 'bsbm_options', 'a:14:{s:16:"bsbm_empty_error";s:64:"Oops! Looks like you answered the security question incorrectly.";s:17:"bsbm_answer_error";s:60:"Oops! It appears you forgot to answer the security question.";s:15:"bsbm_login_form";b:1;s:16:"bsbm_signup_form";b:1;s:17:"bsbm_comment_form";b:1;s:18:"bsbm_register_form";b:1;s:15:"bsbm_mathvalue0";s:1:"2";s:15:"bsbm_mathvalue1";s:2:"15";s:19:"bsbm_notice_message";s:115:"IMPORTANT! To be able to proceed, you need to solve the following simple math (so we know that you are a human) :-)";s:18:"bsbm_hook_location";s:1:"1";s:15:"bsbm_customhook";s:0:"";s:17:"bsbm_override_css";b:0;s:13:"bsbm_tabindex";s:1:"5";s:12:"bsbm_wp_role";s:14:"manage_options";}', 'yes'),
(131, 0, 'bsbm_version', '2.2.2', 'yes'),
(132, 0, 'qtranslate_next_update_mo', '1331157556', 'yes'),
(134, 0, 'ShareAndFollowAdminOptions', 'a:365:{s:7:"cdn-key";s:0:"";s:3:"cdn";a:1:{s:10:"status_txt";s:4:"FAIL";}s:8:"icon_set";N;s:11:"top_padding";s:1:"0";s:13:"print_support";s:4:"true";s:9:"rss_style";s:7:"rss_url";s:5:"nujij";s:2:"no";s:16:"nujij_share_text";s:14:"Share on nuJIJ";s:16:"nujij_popup_text";s:32:"Share this BLOG : TITLE on nuJIJ";s:8:"bookmark";s:2:"no";s:19:"bookmark_share_text";s:19:"Bookmark in Browser";s:19:"bookmark_popup_text";s:26:"Bookmark this BLOG : TITLE";s:6:"sphinn";s:2:"no";s:17:"sphinn_share_text";s:20:"Share this on sphinn";s:17:"sphinn_popup_text";s:33:"Share this BLOG : TITLE on sphinn";s:11:"show_header";s:4:"true";s:15:"follow_location";s:5:"right";s:16:"background_color";s:6:"878787";s:12:"border_color";s:3:"fff";s:12:"follow_color";s:3:"000";s:15:"extra_print_css";s:0:"";s:7:"content";s:0:"";s:19:"twitter_text_suffix";s:0:"";s:21:"width_of_page_minimum";s:0:"";s:9:"extra_css";s:0:"";s:20:"excluded_share_pages";s:0:"";s:10:"list_style";s:8:"iconOnly";s:4:"size";s:2:"32";s:7:"spacing";s:1:"3";s:11:"add_content";s:4:"true";s:10:"add_follow";s:4:"true";s:7:"add_css";s:5:"false";s:8:"post_rss";s:3:"yes";s:8:"facebook";s:3:"yes";s:7:"twitter";s:3:"yes";s:7:"stumble";s:2:"no";s:4:"digg";s:2:"no";s:6:"reddit";s:2:"no";s:5:"hyves";s:2:"no";s:9:"delicious";s:2:"no";s:5:"print";s:2:"no";s:5:"orkut";s:2:"no";s:7:"myspace";s:2:"no";s:11:"google_buzz";s:2:"no";s:10:"yahoo_buzz";s:2:"no";s:15:"yahoo_buzz_link";s:0:"";s:16:"google_buzz_link";s:0:"";s:7:"youtube";s:0:"";s:8:"linkedin";s:2:"no";s:16:"dailymotion_link";s:0:"";s:15:"soundcloud_link";s:0:"";s:15:"foursquare_link";s:0:"";s:14:"vkontakte_link";s:0:"";s:10:"plaxo_link";s:0:"";s:12:"coconex_link";s:0:"";s:12:"gowalla_link";s:0:"";s:9:"xing_link";s:0:"";s:14:"twitter_suffix";s:0:"";s:10:"vimeo_link";s:0:"";s:17:"distance_from_top";s:3:"100";s:19:"follow_list_spacing";s:2:"10";s:9:"vkontakte";s:2:"no";s:4:"mixx";s:2:"no";s:5:"email";s:2:"no";s:11:"tumblr_link";s:0:"";s:6:"tumblr";s:2:"no";s:10:"email_link";s:0:"";s:10:"email_body";s:0:"";s:13:"facebook_link";s:0:"";s:12:"twitter_link";s:0:"";s:12:"stumble_link";s:0:"";s:9:"digg_link";s:0:"";s:11:"reddit_link";s:0:"";s:10:"hyves_link";s:0:"";s:14:"delicious_link";s:0:"";s:10:"orkut_link";s:0:"";s:12:"myspace_link";s:0:"";s:8:"rss_link";N;s:15:"newsletter_link";s:0:"";s:17:"follow_newsletter";s:2:"no";s:5:"cssid";s:1:"5";s:15:"add_follow_text";s:4:"true";s:10:"word_value";s:6:"follow";s:13:"theme_support";s:4:"none";s:17:"follow_list_style";s:8:"iconOnly";s:15:"follow_facebook";s:3:"yes";s:14:"follow_twitter";s:3:"yes";s:14:"follow_stumble";s:2:"no";s:11:"follow_digg";s:2:"no";s:13:"follow_reddit";s:0:"";s:12:"follow_hyves";s:2:"no";s:16:"follow_delicious";s:2:"no";s:12:"follow_orkut";s:2:"no";s:14:"follow_myspace";s:2:"no";s:13:"follow_lastfm";s:2:"no";s:13:"follow_flickr";s:2:"no";s:6:"lastfm";s:0:"";s:18:"follow_google_buzz";s:2:"no";s:15:"follow_linkedin";s:2:"no";s:13:"follow_tumblr";s:2:"no";s:11:"follow_yelp";s:2:"no";s:12:"follow_xfire";s:2:"no";s:17:"follow_yahoo_buzz";s:2:"no";s:16:"follow_vkontakte";s:2:"no";s:12:"follow_plaxo";s:2:"no";s:14:"follow_gowalla";s:2:"no";s:11:"follow_xing";s:2:"no";s:20:"twitter_text_default";s:0:"";s:18:"follow_dailymotion";s:2:"no";s:17:"follow_soundcloud";s:2:"no";s:12:"follow_vimeo";s:2:"no";s:14:"follow_coconex";s:2:"no";s:10:"follow_rss";s:3:"yes";s:14:"follow_youtube";s:2:"no";s:8:"tab_size";s:2:"24";s:10:"css_images";s:2:"no";s:7:"wp_post";s:2:"no";s:7:"wp_page";s:3:"yes";s:7:"wp_home";s:3:"yes";s:10:"wp_archive";s:2:"no";s:9:"wp_author";s:3:"yes";s:6:"bit_ly";s:0:"";s:11:"bit_ly_code";s:0:"";s:17:"follow_foursquare";s:2:"no";s:12:"twitter_text";s:5:"clean";s:14:"add_image_link";s:4:"true";s:13:"default_email";s:0:"";s:9:"word_text";s:7:"follow:";s:19:"default_email_image";s:0:"";s:15:"author_defaults";s:7:"authors";s:14:"logo_image_url";s:0:"";s:12:"homepage_img";s:4:"logo";s:18:"homepage_image_url";s:0:"";s:11:"archive_img";s:4:"logo";s:17:"archive_image_url";s:0:"";s:14:"page_image_url";s:0:"";s:14:"post_image_url";s:0:"";s:8:"page_img";s:4:"logo";s:8:"post_img";s:8:"gravatar";s:10:"share_text";s:6:"share:";s:5:"share";s:2:"no";s:11:"lastfm_link";s:0:"";s:11:"flickr_link";s:0:"";s:13:"linkedin_link";s:0:"";s:10:"xfire_link";s:0:"";s:9:"yelp_link";s:0:"";s:22:"background_transparent";s:2:"no";s:18:"border_transparent";s:2:"no";s:12:"youtube_link";s:0:"";s:18:"css_print_excludes";s:32:"#menu, #navigation, #navi, .menu";s:16:"follow_posterous";s:2:"no";s:9:"follow_ya";s:2:"no";s:14:"posterous_link";s:0:"";s:7:"ya_link";s:0:"";s:17:"css_follow_images";s:2:"no";s:19:"posterous_link_text";s:19:"Check my phone feed";s:17:"follow_feedburner";s:2:"no";s:15:"feedburner_link";s:0:"";s:10:"technorati";s:2:"no";s:21:"technorati_share_text";s:19:"Share on technorati";s:21:"technorati_popup_text";s:37:"Share this BLOG : TITLE on technorati";s:4:"xing";s:2:"no";s:15:"xing_share_text";s:13:"Share on xing";s:15:"xing_popup_text";s:31:"Share this BLOG : TITLE on xing";s:12:"ya_link_text";s:15:"Connect with me";s:17:"follow_slideshare";s:2:"no";s:20:"slideshare_link_text";s:20:"See my presentations";s:15:"slideshare_link";s:0:"";s:16:"follow_wordpress";s:2:"no";s:19:"wordpress_link_text";s:15:"Me on wordpress";s:14:"wordpress_link";s:0:"";s:14:"follow_technet";s:2:"no";s:17:"technet_link_text";s:18:"My technical items";s:12:"technet_link";s:0:"";s:14:"follow_squidoo";s:2:"no";s:17:"squidoo_link_text";s:19:"Check me on Squidoo";s:12:"squidoo_link";s:0:"";s:12:"follow_plurk";s:2:"no";s:15:"plurk_link_text";s:24:"Connect with me on Plurk";s:10:"plurk_link";s:0:"";s:13:"follow_meetup";s:2:"no";s:16:"meetup_link_text";s:19:"Come to the Meeting";s:11:"meetup_link";s:0:"";s:14:"follow_getglue";s:2:"no";s:17:"getglue_link_text";s:22:"Wanna see my stickers?";s:12:"getglue_link";s:0:"";s:11:"follow_ning";s:2:"no";s:14:"ning_link_text";s:22:"Wanna see my stickers?";s:9:"ning_link";s:0:"";s:11:"follow_bebo";s:2:"no";s:14:"bebo_link_text";s:15:"Find me on Bebo";s:9:"bebo_link";s:0:"";s:12:"follow_faves";s:2:"no";s:15:"faves_link_text";s:12:"See my Faves";s:10:"faves_link";s:0:"";s:15:"follow_identica";s:2:"no";s:18:"identica_link_text";s:28:"Connect with me on identi.ca";s:13:"identica_link";s:0:"";s:15:"follow_bandcamp";s:2:"no";s:17:"follow_deviantart";s:2:"no";s:11:"follow_imdb";s:2:"no";s:13:"follow_itunes";s:2:"no";s:12:"follow_moddb";s:2:"no";s:13:"follow_picasa";s:2:"no";s:13:"follow_sphinn";s:2:"no";s:13:"bandcamp_link";s:0:"";s:15:"deviantart_link";s:0:"";s:9:"imdb_link";s:0:"";s:11:"itunes_link";s:0:"";s:10:"moddb_link";s:0:"";s:11:"picasa_link";s:0:"";s:11:"sphinn_link";s:0:"";s:4:"bebo";s:2:"no";s:15:"bebo_share_text";s:13:"Share on bebo";s:15:"bebo_popup_text";s:31:"Share this BLOG : TITLE on bebo";s:8:"identica";s:2:"no";s:19:"identica_share_text";s:17:"Share on identica";s:19:"identica_popup_text";s:35:"Share this BLOG : TITLE on identica";s:5:"dzone";s:2:"no";s:16:"dzone_share_text";s:14:"Share on dzone";s:16:"dzone_popup_text";s:32:"Share this BLOG : TITLE on dzone";s:4:"fark";s:2:"no";s:15:"fark_share_text";s:13:"Share on fark";s:15:"fark_popup_text";s:31:"Share this BLOG : TITLE on fark";s:5:"faves";s:2:"no";s:16:"faves_share_text";s:14:"Share on faves";s:16:"faves_popup_text";s:32:"Share this BLOG : TITLE on faves";s:9:"linkagogo";s:2:"no";s:20:"linkagogo_share_text";s:18:"Share on linkagogo";s:20:"linkagogo_popup_text";s:36:"Share this BLOG : TITLE on linkagogo";s:6:"mrwong";s:2:"no";s:17:"mrwong_share_text";s:15:"Share on mrwong";s:17:"mrwong_popup_text";s:33:"Share this BLOG : TITLE on mrwong";s:8:"netvibes";s:2:"no";s:19:"netvibes_share_text";s:17:"Share on netvibes";s:19:"netvibes_popup_text";s:35:"Share this BLOG : TITLE on netvibes";s:10:"friendfeed";s:2:"no";s:21:"friendfeed_share_text";s:19:"Share on FriendFeed";s:21:"friendfeed_popup_text";s:37:"Share this BLOG : TITLE on FriendFeed";s:15:"friendfeed_ning";s:0:"";s:20:"friendfeed_link_text";s:14:"Check my feeds";s:15:"friendfeed_link";s:0:"";s:17:"follow_friendfeed";s:2:"no";s:25:"wpsc_top_of_products_page";s:2:"no";s:31:"wpsc_product_before_description";s:2:"no";s:30:"wpsc_product_addon_after_descr";s:2:"no";s:12:"follow_email";s:2:"no";s:15:"email_link_text";s:18:"Sign up for emails";s:21:"excluded_follow_pages";s:0:"";s:11:"follow_page";s:3:"yes";s:11:"follow_post";s:3:"yes";s:14:"follow_archive";s:3:"yes";s:11:"follow_home";s:3:"yes";s:13:"follow_author";s:3:"yes";s:20:"vkontakte_share_text";s:18:"Share on vkontakte";s:15:"mixx_share_text";s:10:"Mixx it up";s:19:"linkedin_share_text";s:17:"Share on Linkedin";s:19:"facebook_share_text";s:21:"Recommend on Facebook";s:18:"twitter_share_text";s:14:"Tweet about it";s:17:"tumblr_share_text";s:9:"Tumblr it";s:18:"stumble_share_text";s:20:"Share with Stumblers";s:15:"digg_share_text";s:14:"Digg this post";s:17:"reddit_share_text";s:16:"share via Reddit";s:16:"hyves_share_text";s:12:"Tip on Hyves";s:20:"delicious_share_text";s:21:"Bookmark on Delicious";s:16:"orkut_share_text";s:14:"Share on Orkut";s:18:"myspace_share_text";s:17:"Share via MySpace";s:18:"facebook_link_text";s:25:"Become my Facebook friend";s:17:"twitter_link_text";s:13:"Tweet with me";s:16:"tumblr_link_text";s:16:"Read my Tumbles.";s:15:"xfire_link_text";s:17:"Come on a mission";s:17:"stumble_link_text";s:18:"Follow my Stumbles";s:15:"hyves_link_text";s:25:"Become my friend on Hyves";s:15:"orkut_link_text";s:20:"Become Orkut Buddies";s:17:"myspace_link_text";s:25:"Become a myspace follower";s:20:"foursquare_link_text";s:23:"Follow me on FourSquare";s:20:"soundcloud_link_text";s:18:"Listen to my music";s:20:"feedburner_link_text";s:12:"Stay updated";s:17:"coconex_link_text";s:15:"Connect with us";s:15:"plaxo_link_text";s:20:"Join my address book";s:19:"vkontakte_link_text";s:14:"Become Friends";s:17:"gowalla_link_text";s:17:"Follow my actions";s:14:"xing_link_text";s:15:"Connect with us";s:16:"sphinn_link_text";s:13:"Read my posts";s:16:"itunes_link_text";s:12:"Listen to me";s:20:"deviantart_link_text";s:14:"See my artwork";s:15:"moddb_link_text";s:14:"Gamer? my mods";s:16:"picasa_link_text";s:15:"See my pictures";s:18:"bandcamp_link_text";s:18:"Listen to the band";s:14:"imdb_link_text";s:15:"Read my reviews";s:19:"delicious_link_text";s:16:"See what I share";s:14:"digg_link_text";s:13:"Digg my stuff";s:15:"vimeo_link_text";s:15:"Watch my videos";s:21:"dailymotion_link_text";s:18:"Tune to my channel";s:21:"yahoo_buzz_share_text";s:10:"Buzz it up";s:22:"google_buzz_share_text";s:10:"Buzz it up";s:20:"yahoo_buzz_link_text";s:9:"Follow me";s:16:"lastfm_link_text";s:14:"Check my tunes";s:21:"google_buzz_link_text";s:21:"Join my conversations";s:18:"linkedin_link_text";s:15:"Connect with me";s:14:"yelp_link_text";s:12:"Read reviews";s:16:"flickr_link_text";s:13:"See my photos";s:20:"newsletter_link_text";s:19:"Join the newsletter";s:13:"rss_link_text";s:3:"RSS";s:16:"email_share_text";s:13:"Tell a friend";s:15:"email_body_text";s:40:"here is a link to a site I really like. ";s:17:"youtube_link_text";s:31:"Subscribe to my YouTube Channel";s:19:"post_rss_share_text";s:38:"Subscribe to the comments on this post";s:16:"print_share_text";s:15:"Print for later";s:15:"mixx_popup_text";s:31:"Share this BLOG : TITLE on Mixx";s:19:"linkedin_popup_text";s:35:"Share this BLOG : TITLE on Linkedin";s:19:"facebook_popup_text";s:39:"Recommend this BLOG : TITLE on Facebook";s:18:"stumble_popup_text";s:38:"Share this BLOG : TITLE with Stumblers";s:18:"twitter_popup_text";s:34:"Tweet this BLOG : TITLE on Twitter";s:17:"tumblr_popup_text";s:26:"Tumblr. this BLOG : TITLE ";s:20:"delicious_popup_text";s:39:"Bookmark this BLOG : TITLE on Delicious";s:20:"vkontakte_popup_text";s:36:"Share this BLOG : TITLE on vkontakte";s:15:"digg_popup_text";s:22:"Digg this BLOG : TITLE";s:17:"reddit_popup_text";s:33:"Share this BLOG : TITLE on Reddit";s:16:"hyves_popup_text";s:30:"Tip this BLOG : TITLE on Hyves";s:16:"orkut_popup_text";s:32:"Share this BLOG : TITLE on Orkut";s:18:"myspace_popup_text";s:35:"Share this BLOG : TITLE via MySpace";s:19:"post_rss_popup_text";s:33:"Follow this BLOG : TITLE comments";s:16:"print_popup_text";s:41:"Print this BLOG : TITLE for reading later";s:16:"email_popup_text";s:38:"Tell a friend about this BLOG : TITLE ";s:22:"google_buzz_popup_text";s:26:"Buzz up this BLOG : TITLE ";s:21:"yahoo_buzz_popup_text";s:26:"Buzz up this BLOG : TITLE ";s:18:"blogger_popup_text";s:34:"Share this BLOG : TITLE on Blogger";s:7:"blogger";s:2:"no";s:14:"follow_blogger";s:2:"no";s:18:"blogger_share_text";s:10:"Blog this!";s:12:"blogger_link";s:0:"";s:17:"blogger_link_text";s:20:"Follow me on Blogger";s:12:"like_topleft";s:2:"no";s:13:"like_topright";s:2:"no";s:11:"like_bottom";s:2:"no";s:10:"like_style";s:9:"box_count";s:10:"like_width";s:2:"65";s:10:"like_faces";s:5:"false";s:9:"like_verb";s:4:"like";s:10:"like_color";s:5:"light";s:9:"like_font";s:5:"arial";s:13:"bit_ly_domain";s:0:"";s:13:"tweet_topleft";s:2:"no";s:14:"tweet_topright";s:2:"no";s:12:"tweet_bottom";s:2:"no";s:11:"tweet_width";s:2:"65";s:11:"tweet_style";s:8:"vertical";s:31:"tweet_wpsc_top_of_products_page";s:2:"no";s:37:"tweet_wpsc_product_before_description";s:2:"no";s:36:"tweet_wpsc_product_addon_after_descr";s:2:"no";s:30:"like_wpsc_top_of_products_page";s:2:"no";s:36:"like_wpsc_product_before_description";s:2:"no";s:35:"like_wpsc_product_addon_after_descr";s:2:"no";s:9:"posterous";s:2:"no";s:20:"posterous_share_text";s:18:"Share on Posterous";s:20:"posterous_popup_text";s:36:"Share this BLOG : TITLE on Posterous";s:9:"tweet_via";s:0:"";s:13:"stumble_style";s:1:"5";s:15:"stumble_topleft";s:2:"no";s:16:"stumble_topright";s:2:"no";s:14:"stumble_bottom";s:2:"no";s:33:"stumble_wpsc_top_of_products_page";s:2:"no";s:39:"stumble_wpsc_product_before_description";s:2:"no";s:38:"stumble_wpsc_product_addon_after_descr";s:2:"no";}', 'yes'),
(135, 0, '_site_transient_timeout_wporg_theme_feature_list', '1330417234', 'yes'),
(136, 0, '_site_transient_wporg_theme_feature_list', 'a:0:{}', 'yes'),
(137, 0, 'theme_mods_twentyeleven', 'a:1:{s:16:"sidebars_widgets";a:2:{s:4:"time";i:1330401896;s:4:"data";a:6:{s:19:"wp_inactive_widgets";a:0:{}s:9:"sidebar-1";a:6:{i:0;s:8:"search-2";i:1;s:14:"recent-posts-2";i:2;s:17:"recent-comments-2";i:3;s:10:"archives-2";i:4;s:12:"categories-2";i:5;s:6:"meta-2";}s:9:"sidebar-2";a:0:{}s:9:"sidebar-3";a:0:{}s:9:"sidebar-4";a:0:{}s:9:"sidebar-5";a:0:{}}}}', 'yes'),
(139, 0, 'theme_mods_webonary-zeedisplay', 'a:2:{i:0;b:0;s:18:"nav_menu_locations";a:2:{s:8:"top_navi";i:15;s:9:"main_navi";i:14;}}', 'yes'),
(140, 0, 'theme_switched', '', 'yes'),
(141, 0, 'ShareAndFollowCSS', 'a:3:{s:5:"cssid";s:1:"4";s:5:"print";s:320:"body {background: white;font-size: 12pt;color:black;}\r\n * {background-image:none;}\r\n #wrapper, #content {width: auto;margin: 0 5%;padding: 0;border: 0;float: none !important;color: black;background: transparent none;}\r\n a { text-decoration : underline; color : #0000ff; }\n#menu, #navigation, #navi, .menu {display:none}\n";s:6:"screen";s:2858:".socialwrap li.icon_text a img, .socialwrap li.iconOnly a img, .followwrap li.icon_text a img, .followwrap li.iconOnly a img{border-width:0 !important;background-color:none;}#follow.right {width:32px;position:fixed; right:0; top:100px;background-color:#878787;padding:10px 0;font-family:impact,charcoal,arial, helvetica,sans-serif;-moz-border-radius-topleft: 5px;-webkit-border-top-left-radius:5px;-moz-border-radius-bottomleft:5px;-webkit-border-bottom-left-radius:5px;border:2px solid #fff;border-right-width:0}#follow.right ul {padding:0; margin:0; list-style-type:none !important;font-size:24px;color:black;}\n#follow.right ul li {padding-bottom:10px;list-style-type:none !important;padding-left:4px;padding-right:4px}\n#follow img{border:none;}#follow.right ul li.follow {margin:0 4px;}\n#follow.right ul li.follow img {border-width:0;display:block;overflow:hidden; background:transparent url(http://localhost/wordpress/wp-content/plugins/share-and-follow/images/impact/follow-right.png) no-repeat -0px 0px;height:79px;width:20px;}\n#follow.right ul li a {display:block;}\n#follow.right ul li.follow span, #follow ul li a span {display:none}.share {margin:0 3px 3px 0;}\n.phat span {display:inline;}\nul.row li {float:left;list-style-type:none;}\nli.iconOnly a span.head {display:none}\n#follow.left ul.size16 li.follow{margin:0px auto !important}\nli.icon_text a {padding-left:0;margin-right:3px}\nli.text_only a {background-image:none !important;padding-left:0;}\nli.text_only a img {display:none;}\nli.icon_text a span{background-image:none !important;padding-left:0 !important; }\nli.iconOnly a span.head {display:none}\nul.socialwrap li {margin:0 3px 3px 0 !important;}\nul.socialwrap li a {text-decoration:none;}ul.row li {float:left;line-height:auto !important;}\nul.row li a img {padding:0}.size16 li a,.size24 li a,.size32 li a, .size48 li a, .size60 li a {display:block}ul.socialwrap {list-style-type:none !important;margin:0; padding:0;text-indent:0 !important;}\nul.socialwrap li {list-style-type:none !important;background-image:none;padding:0;list-style-image:none !important;}\nul.followwrap {list-style-type:none !important;margin:0; padding:0}\nul.followwrap li {margin-right:3px;margin-bottom:3px;list-style-type:none !important;}\n#follow.right ul.followwrap li, #follow.left ul.followwrap li {margin-right:0px;margin-bottom:0px;}\n.shareinpost {clear:both;padding-top:0px}.shareinpost ul.socialwrap {list-style-type:none !important;margin:0 !important; padding:0 !important}\n.shareinpost ul.socialwrap li {padding-left:0 !important;background-image:none !important;margin-left:0 !important;list-style-type:none !important;text-indent:0 !important}\n.socialwrap li.icon_text a img, .socialwrap li.iconOnly a img{border-width:0}ul.followrap li {list-style-type:none;list-style-image:none !important;}\ndiv.clean {clear:left;}\ndiv.display_none {display:none;}\n";}', 'yes'),
(142, 0, 'qtranslate_language_names', 'a:18:{s:2:"de";s:7:"Deutsch";s:2:"en";s:7:"English";s:2:"zh";s:6:"中文";s:2:"fi";s:5:"suomi";s:2:"fr";s:9:"Français";s:2:"nl";s:10:"Nederlands";s:2:"sv";s:7:"Svenska";s:2:"it";s:8:"Italiano";s:2:"ro";s:8:"Română";s:2:"hu";s:6:"Magyar";s:2:"ja";s:9:"日本語";s:2:"es";s:8:"Español";s:2:"vi";s:14:"Tiếng Việt";s:2:"ar";s:14:"العربية";s:2:"pt";s:10:"Português";s:2:"pl";s:6:"Polski";s:2:"se";s:4:"Sena";s:2:"th";s:4:"Thai";}', 'yes'),
(143, 0, 'qtranslate_enabled_languages', 'a:2:{i:0;s:2:"en";i:1;s:2:"th";}', 'yes'),
(144, 0, 'qtranslate_default_language', 'en', 'yes'),
(145, 0, 'qtranslate_flag_location', 'plugins/qtranslate/flags/', 'yes'),
(146, 0, 'qtranslate_flags', 'a:18:{s:2:"en";s:6:"gb.png";s:2:"de";s:6:"de.png";s:2:"zh";s:6:"cn.png";s:2:"fi";s:6:"fi.png";s:2:"fr";s:6:"fr.png";s:2:"nl";s:6:"nl.png";s:2:"sv";s:6:"se.png";s:2:"it";s:6:"it.png";s:2:"ro";s:6:"ro.png";s:2:"hu";s:6:"hu.png";s:2:"ja";s:6:"jp.png";s:2:"es";s:6:"es.png";s:2:"vi";s:6:"vn.png";s:2:"ar";s:8:"arle.png";s:2:"pt";s:6:"br.png";s:2:"pl";s:6:"pl.png";s:2:"se";s:6:"se.png";s:2:"th";s:6:"th.png";}', 'yes'),
(147, 0, 'qtranslate_locales', 'a:18:{s:2:"de";s:5:"de_DE";s:2:"en";s:5:"en_US";s:2:"zh";s:5:"zh_CN";s:2:"fi";s:2:"fi";s:2:"fr";s:5:"fr_FR";s:2:"nl";s:5:"nl_NL";s:2:"sv";s:5:"sv_SE";s:2:"it";s:5:"it_IT";s:2:"ro";s:5:"ro_RO";s:2:"hu";s:5:"hu_HU";s:2:"ja";s:2:"ja";s:2:"es";s:5:"es_ES";s:2:"vi";s:2:"vi";s:2:"ar";s:2:"ar";s:2:"pt";s:5:"pt_BR";s:2:"pl";s:5:"pl_PL";s:2:"se";s:5:"se_PR";s:2:"th";s:5:"th_TH";}', 'yes'),
(148, 0, 'qtranslate_na_messages', 'a:18:{s:2:"de";s:58:"Leider ist der Eintrag nur auf %LANG:, : und % verfügbar.";s:2:"en";s:55:"Sorry, this entry is only available in %LANG:, : and %.";s:2:"zh";s:50:"对不起，此内容只适用于%LANG:，:和%。";s:2:"fi";s:92:"Anteeksi, mutta tämä kirjoitus on saatavana ainoastaan näillä kielillä: %LANG:, : ja %.";s:2:"fr";s:65:"Désolé, cet article est seulement disponible en %LANG:, : et %.";s:2:"nl";s:78:"Onze verontschuldigingen, dit bericht is alleen beschikbaar in %LANG:, : en %.";s:2:"sv";s:66:"Tyvärr är denna artikel enbart tillgänglig på %LANG:, : och %.";s:2:"it";s:71:"Ci spiace, ma questo articolo è disponibile soltanto in %LANG:, : e %.";s:2:"ro";s:67:"Din păcate acest articol este disponibil doar în %LANG:, : și %.";s:2:"hu";s:73:"Sajnos ennek a bejegyzésnek csak %LANG:, : és % nyelvű változata van.";s:2:"ja";s:97:"申し訳ありません、このコンテンツはただ今　%LANG:、 :と %　のみです。";s:2:"es";s:68:"Disculpa, pero esta entrada está disponible sólo en %LANG:, : y %.";s:2:"vi";s:63:"Rất tiếc, mục này chỉ tồn tại ở %LANG:, : và %.";s:2:"ar";s:73:"عفوا، هذه المدخلة موجودة فقط في %LANG:, : و %.";s:2:"pt";s:70:"Desculpe-nos, mas este texto esta apenas disponível em %LANG:, : y %.";s:2:"pl";s:68:"Przepraszamy, ten wpis jest dostępny tylko w języku %LANG:, : i %.";s:2:"se";s:54:"Sorry, this entry is only available in %LANG:, : and %";s:2:"th";s:54:"Sorry, this entry is only available in %LANG:, : and %";}', 'yes'),
(149, 0, 'qtranslate_date_formats', 'a:18:{s:2:"en";s:14:"%A %B %e%q, %Y";s:2:"de";s:17:"%A, der %e. %B %Y";s:2:"zh";s:5:"%x %A";s:2:"fi";s:8:"%e.&m.%C";s:2:"fr";s:11:"%A %e %B %Y";s:2:"nl";s:8:"%d/%m/%y";s:2:"sv";s:8:"%Y/%m/%d";s:2:"it";s:8:"%e %B %Y";s:2:"ro";s:12:"%A, %e %B %Y";s:2:"hu";s:12:"%Y %B %e, %A";s:2:"ja";s:15:"%Y年%m月%d日";s:2:"es";s:14:"%d de %B de %Y";s:2:"vi";s:8:"%d/%m/%Y";s:2:"ar";s:8:"%d/%m/%Y";s:2:"pt";s:14:"%d de %B de %Y";s:2:"pl";s:8:"%d/%m/%y";s:2:"se";s:14:"%A %B %e%q, %Y";s:2:"th";s:0:"";}', 'yes'),
(150, 0, 'qtranslate_time_formats', 'a:18:{s:2:"en";s:8:"%I:%M %p";s:2:"de";s:5:"%H:%M";s:2:"zh";s:7:"%I:%M%p";s:2:"fi";s:5:"%H:%M";s:2:"fr";s:5:"%H:%M";s:2:"nl";s:5:"%H:%M";s:2:"sv";s:5:"%H:%M";s:2:"it";s:5:"%H:%M";s:2:"ro";s:5:"%H:%M";s:2:"hu";s:5:"%H:%M";s:2:"ja";s:5:"%H:%M";s:2:"es";s:10:"%H:%M hrs.";s:2:"vi";s:5:"%H:%M";s:2:"ar";s:5:"%H:%M";s:2:"pt";s:10:"%H:%M hrs.";s:2:"pl";s:5:"%H:%M";s:2:"se";s:8:"%I:%M %p";s:2:"th";s:0:"";}', 'yes'),
(151, 0, 'qtranslate_ignore_file_types', 'gif,jpg,jpeg,png,pdf,swf,tif,rar,zip,7z,mpg,divx,mpeg,avi,css,js', 'yes'),
(152, 0, 'qtranslate_url_mode', '1', 'yes'),
(153, 0, 'qtranslate_term_name', 'a:0:{}', 'yes'),
(154, 0, 'qtranslate_use_strftime', '3', 'yes'),
(155, 0, 'qtranslate_detect_browser_language', '1', 'yes'),
(156, 0, 'qtranslate_hide_untranslated', '0', 'yes'),
(157, 0, 'qtranslate_auto_update_mo', '1', 'yes'),
(158, 0, 'qtranslate_hide_default_language', '1', 'yes'),
(159, 0, 'qtranslate_qtranslate_services', '0', 'yes'),
(161, 0, 'nav_menu_options', 'a:2:{i:0;b:0;s:8:"auto_add";a:0:{}}', 'yes'),
(173, 0, '_transient_timeout_dash_20494a3d90a6669585674ed0eb8dcd8f', '1330595169', 'no'),
(174, 0, '_transient_dash_20494a3d90a6669585674ed0eb8dcd8f', '<p><strong>RSS Error</strong>: WP HTTP Error: Could not open handle for fopen() to http://blogsearch.google.com/blogsearch_feeds?scoring=d&ie=utf-8&num=10&output=rss&partner=wordpress&q=link:http://localhost/wordpress/</p>', 'no'),
(187, 0, 'widget_tag_cloud', 'a:2:{i:2;a:2:{s:5:"title";s:6:"Domain";s:8:"taxonomy";s:8:"post_tag";}s:12:"_multiwidget";i:1;}', 'yes');

-- --------------------------------------------------------

--
-- Table structure for table `wp_postmeta`
--

CREATE TABLE IF NOT EXISTS `wp_postmeta` (
  `meta_id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `post_id` bigint(20) unsigned NOT NULL DEFAULT '0',
  `meta_key` varchar(255) DEFAULT NULL,
  `meta_value` longtext,
  PRIMARY KEY (`meta_id`),
  KEY `post_id` (`post_id`),
  KEY `meta_key` (`meta_key`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=61 ;

--
-- Dumping data for table `wp_postmeta`
--

INSERT INTO `wp_postmeta` (`meta_id`, `post_id`, `meta_key`, `meta_value`) VALUES
(1, 2, '_wp_page_template', 'default'),
(2, 2, '_edit_lock', '1330566382:1'),
(3, 2, '_edit_last', '1'),
(4, 5, '_menu_item_type', 'post_type'),
(5, 5, '_menu_item_menu_item_parent', '0'),
(6, 5, '_menu_item_object_id', '2'),
(7, 5, '_menu_item_object', 'page'),
(8, 5, '_menu_item_target', ''),
(9, 5, '_menu_item_classes', 'a:1:{i:0;s:0:"";}'),
(10, 5, '_menu_item_xfn', ''),
(11, 5, '_menu_item_url', ''),
(13, 6, '_menu_item_type', 'custom'),
(14, 6, '_menu_item_menu_item_parent', '0'),
(15, 6, '_menu_item_object_id', '6'),
(16, 6, '_menu_item_object', 'custom'),
(17, 6, '_menu_item_target', ''),
(18, 6, '_menu_item_classes', 'a:1:{i:0;s:0:"";}'),
(19, 6, '_menu_item_xfn', ''),
(20, 6, '_menu_item_url', 'http://localhost/wordpress'),
(22, 7, '_menu_item_type', 'custom'),
(23, 7, '_menu_item_menu_item_parent', '0'),
(24, 7, '_menu_item_object_id', '7'),
(25, 7, '_menu_item_object', 'custom'),
(26, 7, '_menu_item_target', ''),
(27, 7, '_menu_item_classes', 'a:1:{i:0;s:0:"";}'),
(28, 7, '_menu_item_xfn', ''),
(29, 7, '_menu_item_url', 'http://loc'),
(30, 7, '_menu_item_orphaned', '1330406589'),
(31, 8, '_menu_item_type', 'custom'),
(32, 8, '_menu_item_menu_item_parent', '0'),
(33, 8, '_menu_item_object_id', '8'),
(34, 8, '_menu_item_object', 'custom'),
(35, 8, '_menu_item_target', ''),
(36, 8, '_menu_item_classes', 'a:1:{i:0;s:0:"";}'),
(37, 8, '_menu_item_xfn', ''),
(38, 8, '_menu_item_url', 'http://loc'),
(39, 8, '_menu_item_orphaned', '1330406603'),
(59, 1, '_wp_trash_meta_time', '1330406734'),
(49, 10, '_menu_item_type', 'custom'),
(50, 10, '_menu_item_menu_item_parent', '0'),
(51, 10, '_menu_item_object_id', '10'),
(52, 10, '_menu_item_object', 'custom'),
(53, 10, '_menu_item_target', ''),
(54, 10, '_menu_item_classes', 'a:1:{i:0;s:0:"";}'),
(55, 10, '_menu_item_xfn', ''),
(56, 10, '_menu_item_url', 'http://localhost/wordpress/?lang=th'),
(58, 1, '_wp_trash_meta_status', 'publish'),
(60, 1, '_wp_trash_meta_comments_status', 'a:1:{i:1;s:1:"1";}');

-- --------------------------------------------------------

--
-- Table structure for table `wp_posts`
--

CREATE TABLE IF NOT EXISTS `wp_posts` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `post_author` bigint(20) unsigned NOT NULL DEFAULT '0',
  `post_date` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `post_date_gmt` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `post_content` longtext NOT NULL,
  `post_title` text NOT NULL,
  `post_excerpt` text NOT NULL,
  `post_status` varchar(20) NOT NULL DEFAULT 'publish',
  `comment_status` varchar(20) NOT NULL DEFAULT 'open',
  `ping_status` varchar(20) NOT NULL DEFAULT 'open',
  `post_password` varchar(20) NOT NULL DEFAULT '',
  `post_name` varchar(200) NOT NULL DEFAULT '',
  `to_ping` text NOT NULL,
  `pinged` text NOT NULL,
  `post_modified` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `post_modified_gmt` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `post_content_filtered` text NOT NULL,
  `post_parent` bigint(20) unsigned NOT NULL DEFAULT '0',
  `guid` varchar(255) NOT NULL DEFAULT '',
  `menu_order` int(11) NOT NULL DEFAULT '0',
  `post_type` varchar(20) NOT NULL DEFAULT 'post',
  `post_mime_type` varchar(100) NOT NULL DEFAULT '',
  `comment_count` bigint(20) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `post_name` (`post_name`),
  KEY `type_status_date` (`post_type`,`post_status`,`post_date`,`ID`),
  KEY `post_parent` (`post_parent`),
  KEY `post_author` (`post_author`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=14 ;

--
-- Dumping data for table `wp_posts`
--

INSERT INTO `wp_posts` (`ID`, `post_author`, `post_date`, `post_date_gmt`, `post_content`, `post_title`, `post_excerpt`, `post_status`, `comment_status`, `ping_status`, `post_password`, `post_name`, `to_ping`, `pinged`, `post_modified`, `post_modified_gmt`, `post_content_filtered`, `post_parent`, `guid`, `menu_order`, `post_type`, `post_mime_type`, `comment_count`) VALUES
(1, 1, '2012-02-28 03:06:04', '2012-02-28 03:06:04', 'Welcome to WordPress. This is your first post. Edit or delete it, then start blogging!', 'Hello world!', '', 'trash', 'open', 'open', '', 'hello-world', '', '', '2012-02-28 11:25:34', '2012-02-28 05:25:34', '', 0, 'http://localhost/wordpress/?p=1', 0, 'post', '', 1),
(2, 1, '2012-02-28 03:06:04', '2012-02-28 03:06:04', 'This is an example page. It''s different from a blog post because it will stay in one place and will show up in your site navigation (in most themes). Most people start with an About page that introduces them to potential site visitors. It might say something like this:\r\n\r\n<blockquote>Hi there! I''m a bike messenger by day, aspiring actor by night, and this is my blog. I live in Los Angeles, have a great dog named Jack, and I like pi&#241;a coladas. (And gettin'' caught in the rain.)</blockquote>\r\n\r\n...or something like this:\r\n\r\n<blockquote>The XYZ Doohickey Company was founded in 1971, and has been providing quality doohickies to the public ever since. Located in Gotham City, XYZ employs over 2,000 people and does all kinds of awesome things for the Gotham community.</blockquote>\r\n\r\nAs a new WordPress user, you should go to <a href="http://localhost/wordpress/wp-admin/">your dashboard</a> to delete this page and create new pages for your content. Have fun!', '<!--:en-->Home<!--:--><!--:th-->Home<!--:-->', '', 'publish', 'open', 'open', '', 'sample-page', '', '', '2012-03-01 07:44:21', '2012-03-01 01:44:21', '', 0, 'http://localhost/wordpress/?page_id=2', 0, 'page', '', 0),
(3, 1, '2012-02-28 03:06:54', '0000-00-00 00:00:00', '', 'Auto Draft', '', 'auto-draft', 'open', 'open', '', '', '', '', '2012-02-28 03:06:54', '0000-00-00 00:00:00', '', 0, 'http://localhost/wordpress/?p=3', 0, 'post', '', 0),
(4, 1, '2012-02-28 03:06:04', '2012-02-28 03:06:04', 'This is an example page. It''s different from a blog post because it will stay in one place and will show up in your site navigation (in most themes). Most people start with an About page that introduces them to potential site visitors. It might say something like this:\n\n<blockquote>Hi there! I''m a bike messenger by day, aspiring actor by night, and this is my blog. I live in Los Angeles, have a great dog named Jack, and I like pi&#241;a coladas. (And gettin'' caught in the rain.)</blockquote>\n\n...or something like this:\n\n<blockquote>The XYZ Doohickey Company was founded in 1971, and has been providing quality doohickies to the public ever since. Located in Gotham City, XYZ employs over 2,000 people and does all kinds of awesome things for the Gotham community.</blockquote>\n\nAs a new WordPress user, you should go to <a href="http://localhost/wordpress/wp-admin/">your dashboard</a> to delete this page and create new pages for your content. Have fun!', 'Sample Page', '', 'inherit', 'open', 'open', '', '2-revision', '', '', '2012-02-28 03:06:04', '2012-02-28 03:06:04', '', 2, 'http://localhost/wordpress/?p=4', 0, 'revision', '', 0),
(5, 1, '2012-02-28 11:21:35', '2012-02-28 05:21:35', ' ', '', '', 'publish', 'open', 'open', '', '5', '', '', '2012-02-28 11:22:07', '2012-02-28 05:22:07', '', 0, 'http://localhost/wordpress/?p=5', 1, 'nav_menu_item', '', 0),
(6, 1, '2012-02-28 11:22:59', '2012-02-28 05:22:59', '', 'English', '', 'publish', 'open', 'open', '', 'english', '', '', '2012-03-01 04:00:05', '2012-02-29 22:00:05', '', 0, 'http://localhost/wordpress/?p=6', 1, 'nav_menu_item', '', 0),
(7, 1, '2012-02-28 11:23:09', '0000-00-00 00:00:00', '', 'Menu Item', '', 'draft', 'open', 'open', '', '', '', '', '2012-02-28 11:23:09', '0000-00-00 00:00:00', '', 0, 'http://localhost/wordpress/?p=7', 1, 'nav_menu_item', '', 0),
(8, 1, '2012-02-28 11:23:23', '0000-00-00 00:00:00', '', 'Menu Item', '', 'draft', 'open', 'open', '', '', '', '', '2012-02-28 11:23:23', '0000-00-00 00:00:00', '', 0, 'http://localhost/wordpress/?p=8', 1, 'nav_menu_item', '', 0),
(12, 1, '2012-02-28 11:21:08', '2012-02-28 05:21:08', 'This is an example page. It''s different from a blog post because it will stay in one place and will show up in your site navigation (in most themes). Most people start with an About page that introduces them to potential site visitors. It might say something like this:\r\n\r\n<blockquote>Hi there! I''m a bike messenger by day, aspiring actor by night, and this is my blog. I live in Los Angeles, have a great dog named Jack, and I like pi&#241;a coladas. (And gettin'' caught in the rain.)</blockquote>\r\n\r\n...or something like this:\r\n\r\n<blockquote>The XYZ Doohickey Company was founded in 1971, and has been providing quality doohickies to the public ever since. Located in Gotham City, XYZ employs over 2,000 people and does all kinds of awesome things for the Gotham community.</blockquote>\r\n\r\nAs a new WordPress user, you should go to <a href="http://localhost/wordpress/wp-admin/">your dashboard</a> to delete this page and create new pages for your content. Have fun!', '<!--:en-->Home<!--:--><!--:de-->Home<!--:--><!--:fr-->Home<!--:-->', '', 'inherit', 'open', 'open', '', '2-revision-2', '', '', '2012-02-28 11:21:08', '2012-02-28 05:21:08', '', 2, 'http://localhost/wordpress/2-revision-2/', 0, 'revision', '', 0),
(10, 1, '2012-02-28 11:25:00', '2012-02-28 05:25:00', '', 'Thai', '', 'publish', 'open', 'open', '', 'spanish', '', '', '2012-03-01 04:00:05', '2012-02-29 22:00:05', '', 0, 'http://localhost/wordpress/?p=10', 2, 'nav_menu_item', '', 0),
(11, 1, '2012-02-28 03:06:04', '2012-02-28 03:06:04', 'Welcome to WordPress. This is your first post. Edit or delete it, then start blogging!', 'Hello world!', '', 'inherit', 'open', 'open', '', '1-revision', '', '', '2012-02-28 03:06:04', '2012-02-28 03:06:04', '', 1, 'http://localhost/wordpress/?p=11', 0, 'revision', '', 0),
(13, 1, '2012-03-01 04:03:15', '2012-02-29 22:03:15', 'This is an example page. It''s different from a blog post because it will stay in one place and will show up in your site navigation (in most themes). Most people start with an About page that introduces them to potential site visitors. It might say something like this:\r\n\r\n<blockquote>Hi there! I''m a bike messenger by day, aspiring actor by night, and this is my blog. I live in Los Angeles, have a great dog named Jack, and I like pi&#241;a coladas. (And gettin'' caught in the rain.)</blockquote>\r\n\r\n...or something like this:\r\n\r\n<blockquote>The XYZ Doohickey Company was founded in 1971, and has been providing quality doohickies to the public ever since. Located in Gotham City, XYZ employs over 2,000 people and does all kinds of awesome things for the Gotham community.</blockquote>\r\n\r\nAs a new WordPress user, you should go to <a href="http://localhost/wordpress/wp-admin/">your dashboard</a> to delete this page and create new pages for your content. Have fun!', '<!--:en-->Home<!--:--><!--:th-->Home<!--:-->', '', 'inherit', 'open', 'open', '', '2-revision-3', '', '', '2012-03-01 04:03:15', '2012-02-29 22:03:15', '', 2, 'http://localhost/wordpress/?p=13', 0, 'revision', '', 0);

-- --------------------------------------------------------

--
-- Table structure for table `wp_terms`
--

CREATE TABLE IF NOT EXISTS `wp_terms` (
  `term_id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(200) NOT NULL DEFAULT '',
  `slug` varchar(200) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT '',
  `term_group` bigint(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`term_id`),
  UNIQUE KEY `slug` (`slug`),
  KEY `name` (`name`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=16 ;

--
-- Dumping data for table `wp_terms`
--

INSERT INTO `wp_terms` (`term_id`, `name`, `slug`, `term_group`) VALUES
(1, 'Uncategorized', 'uncategorized', 0),
(2, 'Blogroll', 'blogroll', 0),
(3, 'Online Dictionary', 'site-title', 0),
(4, 'Because of the brevity of your search term, partial search was omitted.', 'partial-search-omitted', 0),
(5, 'Click here to include searching through partial words.', 'include-partial-words', 0),
(6, 'Search', 'search', 0),
(7, 'search results for %s', 'search-results-for-s', 0),
(8, 'No search results', 'no-search-results', 0),
(9, 'We did not find any posts containing the string %s', 'we-did-not-find-any-posts', 0),
(10, 'Look up in English or in your native language', 'look-up-languages', 0),
(11, 'All languages', 'all-languages', 0),
(12, 'All parts of speech', 'all-parts-of-speech', 0),
(13, 'Home', 'home', 0),
(14, 'main', 'main', 0),
(15, 'language', 'language', 0);

-- --------------------------------------------------------

--
-- Table structure for table `wp_term_relationships`
--

CREATE TABLE IF NOT EXISTS `wp_term_relationships` (
  `object_id` bigint(20) unsigned NOT NULL DEFAULT '0',
  `term_taxonomy_id` bigint(20) unsigned NOT NULL DEFAULT '0',
  `term_order` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`object_id`,`term_taxonomy_id`),
  KEY `term_taxonomy_id` (`term_taxonomy_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `wp_term_relationships`
--

INSERT INTO `wp_term_relationships` (`object_id`, `term_taxonomy_id`, `term_order`) VALUES
(1, 2, 0),
(2, 2, 0),
(3, 2, 0),
(4, 2, 0),
(5, 2, 0),
(6, 2, 0),
(7, 2, 0),
(1, 1, 0),
(5, 14, 0),
(6, 15, 0),
(10, 15, 0);

-- --------------------------------------------------------

--
-- Table structure for table `wp_term_taxonomy`
--

CREATE TABLE IF NOT EXISTS `wp_term_taxonomy` (
  `term_taxonomy_id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `term_id` bigint(20) unsigned NOT NULL DEFAULT '0',
  `taxonomy` varchar(32) NOT NULL DEFAULT '',
  `description` longtext NOT NULL,
  `parent` bigint(20) unsigned NOT NULL DEFAULT '0',
  `count` bigint(20) NOT NULL DEFAULT '0',
  PRIMARY KEY (`term_taxonomy_id`),
  UNIQUE KEY `term_id_taxonomy` (`term_id`,`taxonomy`),
  KEY `taxonomy` (`taxonomy`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=16 ;

--
-- Dumping data for table `wp_term_taxonomy`
--

INSERT INTO `wp_term_taxonomy` (`term_taxonomy_id`, `term_id`, `taxonomy`, `description`, `parent`, `count`) VALUES
(1, 1, 'category', '', 0, 0),
(2, 2, 'link_category', '', 0, 7),
(3, 3, 'sil_webstrings', '', 0, 1),
(4, 4, 'sil_webstrings', '', 0, 1),
(5, 5, 'sil_webstrings', '', 0, 1),
(6, 6, 'sil_webstrings', '', 0, 1),
(7, 7, 'sil_webstrings', '', 0, 1),
(8, 8, 'sil_webstrings', '', 0, 1),
(9, 9, 'sil_webstrings', '', 0, 1),
(10, 10, 'sil_webstrings', '', 0, 1),
(11, 11, 'sil_webstrings', '', 0, 1),
(12, 12, 'sil_webstrings', '', 0, 1),
(13, 13, 'sil_webstrings', '', 0, 1),
(14, 14, 'nav_menu', '', 0, 1),
(15, 15, 'nav_menu', '', 0, 2);

