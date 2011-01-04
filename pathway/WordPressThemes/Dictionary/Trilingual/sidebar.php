<!-- menu -->
	 <div id="menu">
	<?php if ( !function_exists('dynamic_sidebar') || !dynamic_sidebar('leftsidebar') ) : ?>
			<img src="<?php bloginfo('template_directory'); ?>/images/logo.png"></img>	
			<p>		
			<ul>
			<li><a href="<?php echo get_option('home'); ?>">Home</a></li>
			</ul>
			
			<ul><?php wp_list_pages('title_li='); ?></ul>
			
			<ul>
			<?php /* wp_list_categories('title_li='); */ ?>
			</ul>
			<br />
			
			<?php 
			/*
			if(get_bloginfo('description') != ""){ ?>
		      <h4><?php bloginfo('name'); ?></h4>
			  <p><?php bloginfo('description');			 
			?></p>
			<br />
			<?php } */
			/*
			?>
			<p><a href="<?php bloginfo('rss2_url'); ?>" title="<?php _e('Syndicate this site using RSS'); ?>"><?php _e('<abbr title="Really Simple Syndication"><img src="http://www.feedburner.com/fb/images/pub/feed-icon16x16.png"></abbr>'); ?></a>
			</p>
			<?php if ( function_exists('wp_tag_cloud') ) : ?>
			<!-- available in wp 2.3 -->
			<h4>Popular Tags</h4>
			<p>
			<?php wp_tag_cloud('smallest=8&largest=22&number=10'); ?>
			</p>
			<?php endif; 
			*/
			/*
			?>
			<h4>Archives</h4>
			<ul>
				<?php wp_get_archives('type=monthly'); ?>
			</ul>
			<?php 
			*/
			/* If this is the frontpage */ 
			/*
			if ( is_home() || is_page() ) { ?>
			<h4>Links</h4>
			<ul>
			<?php wp_list_bookmarks('title_li=0&categorize=0'); ?>
			</ul>			
			<?php }
			*/			
			?>			
	<?php endif; ?>
	</div>
	<div class=menubottom><img src="<?php bloginfo('template_directory'); ?>/images/partners.png"></img></div>
<!-- end menu -->