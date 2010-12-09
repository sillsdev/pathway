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
				/**
				 * First, greet the user and prompt for files.
				 */
				case 0 :
					echo '<div class="narrow">';
					echo '<p>' . __( 'Howdy! This importer allows you to import SIL Pathway XHTML data into your WordPress site.',
							'sil-pathway-xhtml-importer' ) . '</p>';
					$this->get_file_names();
					echo '</div>';
					break;
				/**
				 * Second, upload files
				 */
				case 1 :
					check_admin_referer('import-upload');
					//$result = $this->import_xhtml();
					$result = $this->upload_files();
					if (is_wp_error( $result ))
						echo $result->get_error_message();
					break;

			}
			$this->footer();
		}

		/**
		 * Brings up the form to get the files to upload. Based on wp_import_upload_form
		 * in template.php.
		 *
		 * @since 3.0
		 * @param string $action The action attribute for the form.
		 */
		function get_file_names() {
			/** @todo Change the max upload size */
			$bytes = apply_filters( 'import_upload_size_limit', wp_max_upload_size() );
			$size = wp_convert_bytes_to_hr( $bytes );
			$upload_dir = wp_upload_dir();
			if ( ! empty( $upload_dir['error'] ) ) :
				?><div class="error"><p><?php _e('Before you can upload your import file, you will need to fix the following error:'); ?></p>
				<p><strong><?php echo $upload_dir['error']; ?></strong></p></div><?php
			else :
?>
<form enctype="multipart/form-data" id="import-upload-form" method="post" action="<?php echo esc_attr(
		wp_nonce_url("admin.php?import=pathway-xhtml&amp;step=1", 'import-upload')); ?>">
<p>
<label for="upload"><?php _e( 'Choose an XHTML file from your computer:' ); ?></label> (<?php printf( __('Maximum size: %s' ), $size ); ?>)
<input type="file" id="upload" name="import" size="25" />
<input type="hidden" name="action" value="save" />
<input type="hidden" name="max_file_size" value="<?php echo $bytes; ?>" />
</p>
<p class="submit">
<input type="submit" class="button" value="<?php esc_attr_e( 'Upload files and import' ); ?>" />
</p>
</form>
<?php
	endif;
		}

		/**
		 * Header for the screen
		 */
		function header() {
			echo '<div class="wrap">';
			screen_icon();
			echo '<h2>' . __( 'Import SIL Pathway XHTML', 'sil-pathway-xhtml-importer' ) . '</h2>';
		 }

		/**
		 * Footer for the screen
		 */
		function footer() {
			echo '</div>';
		}

		/**
		 * Import the XHTML data
		 *
		 * @return <type>
		 */
		function import_xhtml() {
		$file = wp_import_handle_upload();
			if ( isset($file['error']) ) {
				 echo $file['error'];
				 return;

			}
		}

		/**
		 * Upload the files indicated by the user.
		 *
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
