$("#btnCreateNewModel").click(function () {
    $.ajax({
        url: "/Model/GetSelectListParentModels",
        method: "GET",
        success: function (data) {
            const models = data;
            const inputOptions = {};
            inputOptions["-1"] = "Đây là Model tổng";
            models.forEach((model) => {
                inputOptions[model.id] = model.name;
            });
            //Swal
            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: "btn btn-success",
                    cancelButton: "btn btn-danger",
                },
                buttonsStyling: true,
            });
            swalWithBootstrapButtons
                .fire({
                    title: "Chọn Model tổng",
                    input: "select",
                    inputOptions: inputOptions,
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Xác nhận",
                    cancelButtonText: "Huỷ bỏ",
                    reverseButtons: true,
                })
                .then((result) => {
                    if (result.isConfirmed) {
                        var idModelTong = result.value;
                        const swalWithBootstrapButtons = Swal.mixin({
                            customClass: {
                                confirmButton: "btn btn-success",
                                cancelButton: "btn btn-danger",
                            },
                            buttonsStyling: true,
                        });
                        swalWithBootstrapButtons
                            .fire({
                                title: "Nhập tên Model mới",
                                input: "text",
                                icon: "warning",
                                showCancelButton: true,
                                confirmButtonText: "Xác nhận thêm",
                                cancelButtonText: "Huỷ bỏ",
                                reverseButtons: true,
                            })
                            .then((result) => {
                                if (result.isConfirmed) {
                                    var modelName = result.value;
                                    //AJAX
                                    $.ajax({
                                        type: "POST",
                                        url: "/Model/CreateModel",
                                        data: {
                                            name: modelName,
                                            modelTong: idModelTong,
                                        },
                                        success: function (data) {
                                            if (!data.success) {
                                                console.log(
                                                    "Lỗi xảy ra: " +
                                                        data.message
                                                );
                                                Swal.fire({
                                                    icon: "error",
                                                    title: "Oops...",
                                                    text: data.message,
                                                });
                                            } else {
                                                Swal.fire(
                                                    "Thành công",
                                                    "Bạn vừa thêm Model " +
                                                        modelName +
                                                        " thành công",
                                                    "success"
                                                ).then(function () {
                                                    window.location.reload();
                                                });
                                            }
                                        },
                                        error: function () {
                                            Swal.fire(
                                                "Lỗi máy chủ",
                                                "Thao tác đã được huỷ bỏ!",
                                                "error"
                                            );
                                        },
                                    });
                                } else if (
                                    result.dismiss === Swal.DismissReason.cancel
                                ) {
                                    swalWithBootstrapButtons.fire(
                                        "Đã huỷ",
                                        "Thao tác đã được huỷ bỏ!",
                                        "error"
                                    );
                                }
                            });
                    } else if (result.dismiss === Swal.DismissReason.cancel) {
                        swalWithBootstrapButtons.fire(
                            "Đã huỷ",
                            "Thao tác đã được huỷ bỏ!",
                            "error"
                        );
                    }
                });
        },
        error: function () {
            Swal.fire("Lỗi máy chủ", "Thao tác đã được huỷ bỏ!", "error");
        },
    });
});
