$("#btnXoatemps").click(function () {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-danger",
            cancelButton: "btn btn-success",
        },
        buttonsStyling: true,
    });
    swalWithBootstrapButtons
        .fire({
            title: "Xoá toàn bộ dữ liệu tạm ?",
            text:
                "Tất cả dữ liệu tại folder /temps sẽ bị xoá",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Xác nhận xoá",
            cancelButtonText: "Huỷ bỏ",
            reverseButtons: true,
        })
        .then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/File/ClearTemps",
                    type: "POST",
                    success: function (data) {
                        if (!data.success) {
                            Swal.fire({
                                icon: "error",
                                title: "Oops...",
                                text: "Có lỗi xảy ra",
                            });
                        } else {
                            Swal.fire(
                                "Thành công",
                                "Bạn vừa xoá dữ liệu /temps thành công",
                                "success"
                            );
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
})