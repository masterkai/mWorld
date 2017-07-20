 $(function() {




     // =====================scrollTop=================


     $("#GOTOP").click(function() {
         jQuery("html,body").animate({
             scrollTop: 0
         }, 250);
     });
     $(window).scroll(function() {
         if ($(this).scrollTop() > 100) {
             $('#GOTOP').fadeIn("fast");
         } else {
             $('#GOTOP').stop().fadeOut("fast");
         }
     });








 });


 // =====================watermark=================
 $(document).ready(function() {
     $(":input[data-watermark]").each(function() {
         $(this).val($(this).attr("data-watermark"));
         $(this).bind('focus', function() {
             if ($(this).val() == $(this).attr("data-watermark")) $(this).val('');
         });
         $(this).bind('blur', function() {
             if ($.trim($(this).val()) == '') $(this).val($(this).attr("data-watermark"));
         });
     });
 });
 //=====================ALLBOX=================



 //=====================ALLBOX=================
 $(document).ready(function() {
     $('.popup-with-zoom-anim').magnificPopup({
         type: 'inline',

         fixedContentPos: false,
         fixedBgPos: true,

         overflowY: 'auto',

         closeBtnInside: true,
         preloader: false,

         midClick: true,
         removalDelay: 200,
         mainClass: 'my-mfp-zoom-in'
     });

     // $('.popup-with-move-anim').magnificPopup({
     //     type: 'inline',

     //     fixedContentPos: false,
     //     fixedBgPos: true,

     //     overflowY: 'auto',

     //     closeBtnInside: true,
     //     preloader: false,

     //     midClick: true,
     //     removalDelay: 200,
     //     mainClass: 'my-mfp-slide-bottom'
     // });

     // =====================wow=================
     new WOW().init();

     $('.mfp-close').click(function () {
         $(this).parent().css('display','none');
     });

 });

 var mWorldurl = '/soeasy/activity/phone/1060516m-world/index.html';
//  var xhr = new XMLHttpRequest();
//  xhr.open('GET', mWorldurl, true);
//  xhr.onload = function() {
//      if (xhr.status >= 200 && xhr.status < 400) {
// // Success!
//          console.log(xhr.responseText);
//      }
//  };
// xhr.send();
 //regex for table element  /(<table[^>]*>(?:.|\n)*?<\/table>)/gi

 $.get(mWorldurl, function (data) {
     $('#tabletext').val(data);
     var tContent = $('#tabletext').val();
     // console.log(tContent);
     var tablesRegex = /(<table[^>]*>(?:.|\n)*?<\/table>)/g;
     var tContentArray = tContent.match(tablesRegex);
     // console.log(tContentArray.length);
     console.log($('.POP'));
     // console.log(tContentArray[0]);
     for(var i = 0; tContentArray.length>i;i++){
         // console.log(tContentArray[i]);
         $('.POP').eq(i).html(tContentArray[i]);
     }
 });

// var tContent = xhr.responseText;

// $('.pop').each(function(idx,pobj){
//     console.log(idx + )
//  });



