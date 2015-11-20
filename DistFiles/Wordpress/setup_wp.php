<?php
		/**
 * We are installing WordPress.
 *
 * @since 1.5.1
 * @var bool
 */
define( 'WP_INSTALLING', true );

/** Load WordPress Bootstrap */
require_once( dirname( dirname( __FILE__ ) ) . '/wp-load.php' );

/** Load WordPress Administration Upgrade API */
require_once( dirname( __FILE__ ) . '/includes/upgrade.php' );

/** Load wpdb */
require_once(dirname(dirname(__FILE__)) . '/wp-includes/wp-db.php');

		
		// Fill in the data we gathered
$weblog_title = isset( $_GET['weblog_title'] ) ? trim( stripslashes( $_GET['weblog_title'] ) ) : '';
$user_name = isset($_GET['user_name']) ? trim( stripslashes( $_GET['user_name'] ) ) : 'admin';
$admin_password = isset($_GET['admin_password']) ? $_GET['admin_password'] : '';
$admin_email  = isset( $_GET['admin_email']  ) ?trim( stripslashes( $_GET['admin_email'] ) ) : '';
$public       = isset( $_GET['blog_public']  ) ? (int) $_GET['blog_public'] : 0;

$result = wp_install($weblog_title, $user_name, $admin_email, $public, '', $admin_password);
?>