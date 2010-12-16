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

/* @todo Change the above Plugin URI */
/* @todo Change the licensing above and below. If GPL2, see WP plugin doc about license. */

/*
 * Check to make sure we can even load an importer.
 */

if ( ! defined( 'WP_LOAD_IMPORTERS' ) )
    return;

/*
 * Include the WordPress Importer.
 */
require_once ABSPATH . 'wp-admin/includes/import.php';

if ( ! class_exists('WP_Importer') )
    {
    $class_wp_importer = ABSPATH . 'wp-admin/includes/class-wp-importer.php';
    if ( file_exists( $class_wp_importer ) )
        require_once $class_wp_importer;
    }

if ( class_exists( 'WP_Importer' ) ) {

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

	class SIL_Pathway_XHTML_Import extends WP_Importer {
		var $posts = array ();
		var $file;

		function start()
		{
			/* @todo See if there is a better way to do this than these steps */
			if ( empty ( $_GET['step'] ) )
				$step = 0;
			else
				$step = (int) $_GET['step'];

			$this->header();

			switch ($step) {
				/*
				 * First, greet the user and prompt for files.
				 */
				case 0 :
					$this->hello();
					$this->get_file_names();
					echo '</div>';
					break;
				/*
				 * Second, upload and import files
				 */
				case 1 :
					check_admin_referer('import-upload');
					$result = $this->upload_files('xhtml');
					if (is_wp_error( $result ))
						echo $result->get_error_message();
					$xhtml_file = $result['file'];

					$result = $this->upload_files('css');
					if (is_wp_error( $result ))
						echo $result->get_error_message();

					$result = $this->import_xhtml($xhtml_file);

					$this->goodbye();
					break;

			}
			$this->footer();
		}

		/**
		 * Greet the user.
		 */
		function hello(){
			echo '<div class="narrow">';
			echo '<p>' . __( 'Howdy! This importer allows you to import SIL Pathway XHTML data into your WordPress site.',
					'sil-pathway-xhtml-importer' ) . '</p>';
		}

		/**
		 * Finish up.
		 */
		function goodbye(){
			echo '<div class="narrow">';
			echo '<p>' . __( 'Finished!', 'sil-pathway-xhtml-importer' ) . '</p>';
		}
		/**
		 * Brings up the form to get the files to upload. The code is based on
		 * the function wp_import_upload_form in template.php.
		 *
		 * @since 3.0
		 */
		function get_file_names() {
			
			/* @todo The max file size is determined by the settings in php.ini.
			 * upload_max_files is set to 2MB by default. This can be cranked up,
			 * but then the post_max_size apparently needs to be at least as big
			 * as the upload_max_files setting. This all assumes that the
			 * settings are accessible. Will they? Note that if the file size
			 * is bigger than the limit, the server simply will not upload it,
			 * and there is no indication to the user as to what happened.
			 *
			 * If the limitation will continue, we have one of two options:
			 *
			 * 1. The program will need to get a lot smarter and chunk the XHTML
			 * into smaller pieces. It will upload each piece and then import it
			 * all.
			 * 
			 * 2. The user will just have to FTP files up to the server.
			 *
			 * This issue is logged in Jira TD-1727.
			 */
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
</p>
<p>
<input type="file" id="upload" name="xhtml" size="100" />
</p>
<p>
<label for="upload"><?php _e( 'Choose the associated CSS file from your computer:' ); ?></label> (<?php printf( __('Maximum size: %s' ), $size ); ?>)
</p>
<p>
<input type="file" id="upload" name="css" size="100" />
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
		 * Upload the files indicated by the user. An override of
		 * wp_import_handle_upload.
		 *
		 * @param string $which_file = The file being uploaded
		 * @return array $file = the file, $id = the file's ID
		 */

		function upload_files($which_file) {
			if ( !isset($_FILES[$which_file]) ) {
				$file['error'] = __( 'The file is either empty, or uploads are disabled in your php.ini, or post_max_size is defined as smaller than upload_max_filesize in php.ini.' );
				return $file;
			}

			$overrides = array( 'test_form' => false, 'test_type' => false );
			$file = wp_handle_upload( $_FILES[$which_file], $overrides );

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

		/**
		 * Import the XHTML data
		 *
		 * @return <type>
		 */
		function import_xhtml($xhtml_file) {
			$xhtml_file = realpath($xhtml_file);
			$dom = new DOMDocument('1.0', 'utf-8');
			$ret_val = $dom->load($xhtml_file);
			$xpath = new DOMXPath($dom);
			$xpath->registerNamespace('xhtml', 'http://www.w3.org/1999/xhtml');
			$entry_nodes = $xpath->query('//xhtml:div[@class="entry"]');
			
			/*
			 * Loop through the entries so we can post them to WordPress.
			 */
			foreach ($entry_nodes as $entry_element){
				/*
				 * should be only 1 headword at most
				 */
				$headwords = $xpath->query('./xhtml:span[@class="headword"]', $entry_element);
				$headword = $headwords->item(0)->nodeValue;
				
				$entry_xml = $dom->saveXML($entry_element);

//				$senses = $xpath->query('./xhtml:span[@class="senses"]/xhtml:span[@class="sense"]', $entry_element);
//				foreach ($senses as $sense){
//					$definition = $xpath->query('./xhtml:span[starts-with(@class,"definition")]', $sense)->item(0);
//				}

				/*
				 * Build the post to put into WordPress
				 */
				$post = array(
					'post_title' => $headword,
					'post_content' => $entry_xml,
					'post_status' => 'publish'
				);
				wp_insert_post($post);
			}

			return;
		}

		function SIL_Pathway_XHTML_Import()
        {
			/**
			 * Empty function
			 */
		}
	} // class

	/*
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
	/*
	 * Load the translated strings for the plugin.
	 */
    load_plugin_textdomain('sil-pathway-xhtml-importer', false, dirname( plugin_basename( __FILE__ ) ) );
}

/*
 * Hook the importer's init into the WordPress init.
 */
add_action('init', 'pathway_xhtml_importer_init');
