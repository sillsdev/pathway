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
			/** @todo See if there is a better way to do this than these steps */
			if ( empty ( $_GET['step'] ) )
				$step = 0;
			else
				$step = (int) $_GET['step'];

			$this->header();

			switch ($step) {
				case 0 :
					$this->greet_getfiles();
					break;
				case 1 :
					/**
					 * wp_nonce_field has been set by wp_import_upload-form.
					 * check_admin_referer to see if something succeeded.
					 */
					check_admin_referer('import-upload');
					//$result = $this->import_xhtml();
					$result = $this->upload_files();
					if (is_wp_error( $result ))
						echo $result->get_error_message();
					break;

			}
			$this->footer();
		}

		function greet_getfiles() {
			echo '<div class="narrow">';
			echo '<p>' . __( 'Howdy! This importer allows you to import SIL Pathway XHTML data into your WordPress site.',
					'sil-pathway-xhtml-importer' ) . '</p>';
			/**
			 * wp_import_upload_form looks like it does what I want it to,
			 * but the parameter is a bit of a mystery at the moment. (I pulled
			 * the idea of the call from rss-importer.php.) The function lives
			 * in template.php.
			 */
			wp_import_upload_form("admin.php?import=pathway-xhtml&amp;step=1");
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

		function import_xhtml() {
			$file = wp_import_handle_upload();
			if ( isset($file['error']) ) {
				echo $file['error'];
				return;
			}
		}

		/**
		 * This is intended to be an override of wp_import_handle_upload.
		 * For some reason it's still calling the underlying function.
		 */

		function upload_files() {
			if ( !isset($_FILES['import']) ) {
				$file['error'] = __( 'The file is either empty, or uploads are disabled in your php.ini, or post_max_size is defined as smaller than upload_max_filesize in php.ini.' );
				return $file;
			}

			$overrides = array( 'test_form' => false, 'test_type' => false );
			$file = wp_handle_upload( $_FILES['import'], $overrides );

			if ( isset( $file['error'] ) )
				return $file;

			$url = $file['url'];
			$type = $file['type'];
			$file = addslashes( $file['file'] );
			$filename = basename( $file );

			// Construct the object array
			$object = array( 'post_title' => $filename,
				'post_content' => $url,
				'post_mime_type' => $type,
				'guid' => $url
			);

			// Save the data
			$id = wp_insert_attachment( $object, $file );

			return array( 'file' => $file, 'id' => $id );
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
	register_importer('pathway-xhtml',
			__('SIL Pathway XHTML', 'sil-pathway-xhtml-importer'),
			__('Import posts from an SIL Pathway XHTML file.', 'sil-pathway-xhtml-importer'),
			array ($pathway_import, 'start'));

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
