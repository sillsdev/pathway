<?php get_header(); ?>
  <div id="bd" class="yui-navset">
	               
    <div id="yui-main">
           <div class="yui-b" >
			<div class="yui-ge"><div class="yui-u first">
                        
 
        <?php if (have_posts()) : ?>
		<div class="item entry">
	  <?php $post = $posts[0]; // Hack. Set $post so that the_date() works. ?>
 	  <?php /* If this is a category archive */ if (is_category()) { ?>
		<h2 class="pagetitle">Archive for the &#8216;<?php single_cat_title(); ?>&#8217; category</h2>
 	  <?php /* If this is a tag archive */ } elseif( function_exists('is_tag') ) { if(is_tag()){ ?>
		<h2 class="pagetitle">Posts tagged &#8216;<?php single_tag_title(); ?>&#8217;</h2>
 	  <?php /* If this is a daily archive */} } elseif (is_day()) { ?>
		<h2 class="pagetitle">Archive for <?php the_time('F jS, Y'); ?></h2>
 	  <?php /* If this is a monthly archive */ } elseif (is_month()) { ?>
		<h2 class="pagetitle">Archive for <?php the_time('F, Y'); ?></h2>
 	  <?php /* If this is a yearly archive */ } elseif (is_year()) { ?>
		<h2 class="pagetitle">Archive for <?php the_time('Y'); ?></h2>
	  <?php /* If this is an author archive */ } elseif (is_author()) { ?>
		<h2 class="pagetitle">Author archive</h2>
 	  <?php /* If this is a paged archive */ } elseif (isset($_GET['paged']) && !empty($_GET['paged'])) { ?>
		<h2 class="pagetitle">Blog archives</h2>
 	  <?php } ?>
		</div>
 
                <?php while (have_posts()) : the_post(); ?>
<!-- item -->
                                <div class="item entry" id="post-<?php the_ID(); ?>">
                                          <div class="itemhead">
                                            <h3><a href="<?php the_permalink() ?>" rel="bookmark"><?php the_title(); ?></a></h3>
                                            <div class="chronodata"><?php the_time('F jS, Y') ?> <!-- by <?php the_author() ?> --></div>
                                          </div>
                                                  <div class="storycontent">
                                                                <?php the_excerpt(); ?>
                                                  </div>
                                          <small class="metadata">
                                                         <span class="category">Filed under: <?php the_category(', ') ?> </span> | <?php edit_post_link('Edit', '', ' | '); ?> <?php comments_popup_link('Comment (0)', ' Comment (1)', 'Comments (%)'); ?>
                                                 <?php if ( function_exists('wp_tag_cloud') ) : ?>
							 <?php the_tags('<span class="tags">Article tags: ', ', ' , '</span>'); ?>
							 <?php endif; ?>
												  </small>
                                 </div>
<!-- end item -->

 
 
<?php comments_template(); // Get wp-comments.php template ?>
 
                <?php endwhile; ?>
 
                <div class="navigation">
                        <div class="alignleft"><?php next_posts_link('&laquo; Previous Entries') ?></div>
                        <div class="alignright"><?php previous_posts_link('Next Entries &raquo;') ?></div>
                        <p> </p>
                </div>
 
        <?php else : ?>
 
                <h2 class="center">Not Found</h2>
                <p class="center">Sorry, but you are looking for something that has no entries.</p>
 
        <?php endif; ?>
<!-- end content -->
<!-- 2nd sidebar -->
</div><!-- end yiu-u --><div class="yui-u" id="third"><?php if ( !function_exists('dynamic_sidebar') || !dynamic_sidebar('rightsidebar') ) : ?><h4>Extra Column</h4><p>You can fill this column by editing the index.php theme file. Or by Widget support.</p><?php endif; ?></div>
<!-- end 2nd sidebar -->
                        </div>
                </div>
        </div>
        <div class="yui-b" id="secondary">
 
<?php get_sidebar(); ?>
 
        </div>
  </div>
<?php get_footer(); ?>