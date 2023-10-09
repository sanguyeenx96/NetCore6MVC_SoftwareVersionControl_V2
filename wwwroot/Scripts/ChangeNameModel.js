$(".dropdown-item.changeNameModel").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    var id = $(this).data("id");

    $.ajax({
        url: "/Model/checkToChangeNameModel",
        type: "GET",
        data: { id: id },
        success: function (data) {
            if (data.success) {
                Swal.fire({
                    title: "Đổi tên Model",
                    html: `
                        <input type="text" id="newName" class="swal2-input" placeholder="Nhập tên mới" autocomplete="off" >
                        `,
                    confirmButtonText: "Xác nhận",
                    focusConfirm: false,
                    preConfirm: () => {
                        const tenMoi =
                            Swal.getPopup().querySelector("#newName").value;
                        if (!tenMoi) {
                            Swal.showValidationMessage(
                                `Phải nhập đầy đủ thông tin`
                            );
                        }
                        return { tenMoi: tenMoi };
                    },
                }).then((result) => {
                    var tenMoi = result.value.tenMoi.trim();
                    $.ajax({
                        url: "/Model/changeNameModel",
                        type: "POST",
                        data: { id: id, tenMoi: tenMoi },
                        success: function () {
                            Swal.fire(
                                "Thành công",
                                "Đã đổi tên Model thành công!",
                                "success"
                            ).then(function () {
                                window.location.reload();
                            });
                        },
                        error: function (xhr, t, error) {
                            console.log(error);
                            Swal.fire(
                                "Không lấy được dữ liệu",
                                "Thao tác đã được huỷ bỏ!",
                                "error"
                            );
                        },
                    });
                });
            } else {
                Swal.fire(
                    "Model đã có dữ liệu phần mềm",
                    "Chỉ có thể đổi tên Model chưa có dữ liệu phần mềm!",
                    "error"
                );
            }
        },
        error: function (xhr, t, error) {
            console.log(error);
            Swal.fire(
                "Không lấy được dữ liệu",
                "Thao tác đã được huỷ bỏ!",
                "error"
            );
        },
    });
});
