$(document).ready(function () {
    $(document).on("click", ".trash-icon i", function () {

        let imageId = $(this).parent().attr("data-id");
        let deletedElem = $(this).parent();
        let data = { id: imageId };

        $.ajax({
            url: "/Admin/Course/DeleteCourseImage",
            type: "POST",
            data: data,
            success: function (res) {
                if (res) {
                    $(deletedElem).remove();
                }

                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Course must have at least 1 image'
                    })
                }

            }

        })
    })
})