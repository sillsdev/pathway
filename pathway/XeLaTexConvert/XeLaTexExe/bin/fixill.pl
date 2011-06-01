#!perl -w
#
#   Fix %%BeginData comment in Illustrator files from Illustrator
#   8.0 and 9.0.
#
#   Usage:
#
#   perl fixill.pl <oldfile.eps >newfile.eps
#
binmode STDIN ;
binmode STDOUT ;
my $remainder = "" ;
sub getline {
   my $r = $remainder ;
   $remainder = "" ;
   while (defined($c=getc())) {
      $r .= $c ;
      if ($c eq "\r") { # gotta look ahead here
         if (defined($c = getc())) {
            if ($c eq "\n") {
               $r .= $c ;
            } else {
               $remainder = $c ;
            }
         }
         return $r ;
      } elsif ($c eq "\n") {
         return $r ;
      }
   }
   return $r ;
}
@a = () ;
while (($ln = getline()) ne "") {
   push @a, $ln ;
}
for ($i=0; $i<@a; $i++) {
   if (($bc, undef, undef, $what) =
                       ($a[$i] =~ /^%%BeginData:\s+(\S+)(\s+(\S+)\s+(\S+))?/)) {
      if (!defined($what) || lc $what eq "bytes") {
         my $beg = $i ;
         my $rbc = 0 ;
         $i++ ;
         while ($i < @a && $a[$i] !~ /^%%EndData/) {
            $rbc += length($a[$i++]) ;
         }
         if ($bc != $rbc) {
            $a[$beg] =~ s/$bc/$rbc/ ;
            print STDERR "Byte count updated from $bc to $rbc\n" ;
         }
      }
   }
}
for (@a) {
   print ;
}
