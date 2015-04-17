$(document).ready(function () {
	
	CKEDITOR.on('instanceReady', function(ev) {
		ev.editor.on('paste', function(evt) {   
			evt.data.dataValue = '<!--class="Mso"-->'+evt.data.dataValue;
		}, null, null, 9);
	});
	
	var current_editor = ''; 
	CKEDITOR.on( 'currentInstance', function() {
		if (current_editor != CKEDITOR.currentInstance && current_editor != null && current_editor.container){
			var id = $(current_editor.container.$).attr('id');
			$.ajax({
				url: 'ckeditor/save-to-file.php',
				data: 'contentId='+id+'&content='+encodeURIComponent(current_editor.getData()),
				async: true,
				type: "POST"
			});
			
		}
		current_editor = CKEDITOR.currentInstance;
	} );

});