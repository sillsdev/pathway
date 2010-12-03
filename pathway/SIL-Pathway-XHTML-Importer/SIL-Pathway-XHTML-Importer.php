<?php
/*
Plugin Name: SIL Pathway XHTML Importer
Plugin URI: http://code.google.com/p/pathway/
Description: Imports data from an SIL Pathway XHTML file.
Author: SIL International
Author URI: http://www.sil.org/
Version: 0.1
Stable tag: 0.1
License: GPL v2 - http://www.gnu.org/licenses/old-licenses/gpl-2.0.html
*/

/** @todo Change the above Plugin URI */
/** @todo Change the licensing above and below. If GPL2, see WP plugin doc about license. */

/**
 * Check to make sure we can even load an importer.
 */

if ( ! defined( 'WP_LOAD_IMPORTERS' ) )
    return;

/**
 * Include the WordPress Importer.
 */
require_once ABSPATH . 'wp-admin/includes/import.php';

if ( ! class_exists('WP_Importer') )
    {
    $class_wp_importer = ABSPATH . 'wp-admin/includes/class-wp-importer.php';
    if ( file_exists( $class_wp_importer ) )
        require_once $class_wp_importer;
    }

/**
 * SIL FieldWorks XHTML Importer
 *
 * Imports data from an SIL Pathway XHTML file. The data may come from SIL
 * FieldWorks or other applications.
 *
 * PHP version 5.2
 *
 * LICENSE
 *
 * @package WordPress
 * @subpackage Importer
 * @since 3.0
 */

if ( class_exists( 'WP_Importer' ) ) {
	class SIL_Pathway_XHTML_Import extends WP_Importer {
		var $posts = array ();
		var $file;

		function start()
		{
			if ( empty ( $_GET['step'] ) )
				$step = 0;
			else
				$step = (int) $_GET['step'];

			$this->header();

			switch ($step) {
				case 0 :
					$this->greet();
					break;
				case 1 :
					/**
					 * Presumably wp_nonce_field has been set somewhere for
					 * check_admin_referer to see if something succeeded.
					 */
					check_admin_referer('import-upload');
//					$result = $this->import();
//					if (is_wp_error( $result ))
//						echo $result->get_error_message();
					break;

			}
			$this->footer();
		}

		function greet() {
			echo '<div class="narrow">';
			echo '<p>' . __( 'Howdy! This importer allows you to import SIL Pathway XHTML data into your WordPress site.', 'sil-pathway-xhtml-importer' ) . '</p>';
			//wp_import_upload_form("admin.php?import=rss&amp;step=1");
			echo '</div>';
		}

		function header() {
			echo '<div class="wrap">';
			screen_icon();
			echo '<h2>' . __( 'Import SIL Pathway XHTML', 'sil-pathway-xhtml-importer' ) . '</h2>';
		 }

		 function footer() {
			 echo '</div>';
		 }

		function _normalize_tag($matches) {
			return '<' . strtolower( $matches[1] );
		}
		
		function get_posts() {
			global $wpdb;
			/** TODO Complete this */
		}
		
		function import_posts() {
			echo '<ol>';
			
			foreach ($this->posts as $post) {
				echo "<li>" . __( 'Importing post...', 'sil-pathway-xhtml-importer');

				extract($post);
				$post_id = post_exists($post_title, $post_content, $post_date);
				if ( $post_id ) {
					_e('Post already imported', 'sil-pathway-xhtml-importer');
					
				} else {
					$post_id = wp_insert_post($post);
					if ( is_wp_error( $post_id ) )
						return $post_id;
					if (!$post_id) {
						_e('Couldn&#8217;t get post ID', 'sil-pathway-xhtml-importer');
						return;
					}

					if (0 != count($categories))
						wp_create_categories($categories, $post_id);

					_e('Done!', 'sil-pathway-xhtml-importer');

				}
				echo '</li>';
			}
			echo '</ol>';
		}
		
		function import() {
			$file = wp_import_handle_upload();
			if ( isset($file['error']) ) {
				echo $file['error'];
				return;
			}

			$this->file = $file['file'];
			$this->get_posts();
			$result = $this->import_posts();
			if ( is_wp_error( $result ) )
				return $result;
			wp_import_cleanup($file['id']);
			do_action('import_done', 'rss');
			
			echo '<h3>';
			printf(__('All done. <a href="%s">Have fun!</a>', 'sil-pathway-xhtml-importer'), get_option('home'));
			echo '</h3>';

		}
		
		function SIL_Pathway_XHTML_Import()
        {
			/**
			 * Empty function
			 */
		}
	} // class

	/**
	 * Register the importer so WordPress knows it exists. Specify the start
	 * function as an entry point. Paramaters: $id, $name, $description,
	 * $callback.
	 */
	$pathway_import = new SIL_Pathway_XHTML_Import();
	register_importer('pathway-xhtml', __('SIL Pathway XHTML', 'sil-pathway-xhtml-importer'), __('Import posts from an SIL Pathway XHTML file.', 'sil-pathway-xhtml-importer'), array ($pathway_import, 'start'));

} // class_exists( 'WP_Importer' )

function pathway_xhtml_importer_init() {
	/**
	 * Load the translated strings for the plugin.
	 */
    load_plugin_textdomain('sil-pathway-xhtml-importer', false, dirname( plugin_basename( __FILE__ ) ) );
}

/**
 * Hook the importer's init into the WordPress init.
 */
add_action('init', 'pathway_xhtml_importer_init');
