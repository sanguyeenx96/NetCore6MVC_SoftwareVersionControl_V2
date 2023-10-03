$(".dropdown-item.delete").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    var itemId = $(this).closest("tr").data("item-id");
    var clickedRowValue = $(this).closest("tr").find("td:first").text().trim();
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-danger",
            cancelButton: "btn btn-success",
        },
        buttonsStyling: true,
    });
    swalWithBootstrapButtons
        .fire({
            title: "Xoá Model " + clickedRowValue + " ?",
            text:
                "Tất cả các Model thuộc Model " +
                clickedRowValue +
                " sẽ bị xoá",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Xác nhận xoá",
            cancelButtonText: "Huỷ bỏ",
            reverseButtons: true,
        })
        .then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/Model/DeleteModel",
                    type: "DELETE",
                    data: { id: itemId },
                    success: function (data) {
                        if (!data.success) {
                            console.log("Lỗi xảy ra: " + data.message);
                            Swal.fire({
                                icon: "error",
                                title: "Oops...",
                                text: data.message,
                            });
                        } else {
                            Swal.fire(
                                "Thành công",
                                "Bạn vừa xoá " +
                                    clickedRowValue +
                                    " thành công",
                                "success"
                            ).then(function () {
                                window.location.reload();
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: error,
                        });
                    },
                });
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                swalWithBootstrapButtons.fire(
                    "Đã huỷ",
                    "Thao tác đã được huỷ bỏ!",
                    "error"
                );
            }
        });
});
