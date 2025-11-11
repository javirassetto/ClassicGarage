$("#selec").change(function(){

  readURL(this);
});

function readURL(input){
    if(input.files && input.file[0]) {
        var reader = new FileReader();

        reader.onload=function(e) {
            $("#imagen").attr("src", e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

<script>
    function previewImage(event) {
        var input = event.target;
    var reader = new FileReader();

    reader.onload = function () {
            var dataURL = reader.result;
    var imagePreview = document.getElementById('imagePreview');
    imagePreview.src = dataURL;
    imagePreview.style.display = 'block';
        };

    reader.readAsDataURL(input.files[0]);
    }
</script>

