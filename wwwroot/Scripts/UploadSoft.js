
var nameFile = "";
var ModelId = "";
var SoftId = "";

//Dropzone và upload phần mềm:
Dropzone.autoDiscover = false;
var previewNode = document.querySelector("#template");
previewNode.id = "";
var previewTemplate = previewNode.parentNode.innerHTML;
previewNode.parentNode.removeChild(previewNode);
var myDropzone = new Dropzone(document.body, {
    // Make the whole body a dropzone
    url: "/upload", // Set the url
    maxFiles: 1,
    maxFilesize: 1000, //500MB
    parallelUploads: 20,
    previewTemplate: previewTemplate,
    autoQueue: false, // Make sure the files aren't queued until manually added
    previewsContainer: "#previews", // Define the container to display the previews
    clickable: ".fileinput-button", // Define the element that should be used as click trigger to select files.
});
myDropzone.on("addedfile", function (file) {
    file.previewElement.querySelector(".start").onclick = function () {
        myDropzone.enqueueFile(file);
    };
});
myDropzone.on("totaluploadprogress", function (progress) {
    document.querySelector("#total-progress .progress-bar").style.width =
        progress + "%";
});
myDropzone.on("sending", function (file, xhr, data) {
    document.querySelector("#total-progress").style.opacity = "1";
    file.previewElement
        .querySelector(".start")
        .setAttribute("disabled", "disabled");
    if (file.fullPath) {
        data.append("fullPath", file.fullPath);
    }
});
myDropzone.on("queuecomplete", function (progress) {
    document.querySelector("#total-progress").style.opacity = "0";
});
myDropzone.on("success", function (file) {
    file.previewElement.querySelector(".start").style.display = "none";
    file.previewElement.querySelector(".cancel").style.display = "none";
    file.previewElement.querySelector(".delete").style.display = "none";
    var successElement = file.previewElement.querySelector(
        "strong.success.text-success"
    );
    successElement.textContent = "Tập tin đã tải lên thành công";
    nameFile = file.name;
    console.log(nameFile);
    console.log(file);
    Swal.fire("Thành công", "Tập tin đã được tải lên thành công!", "success");
});
myDropzone.on("error", function (file, errorMessage) {
    Swal.fire("Thất bại", errorMessage, "error");
    file.status = Dropzone.ERROR;
    file.previewElement.querySelector(".start").style.display = "none";
    file.previewElement.querySelector(".cancel").style.display = "none";
    file.previewElement.querySelector(".delete").style.display = "none";
    file.previewElement.querySelector(".progress-bar").style.display = "none";
});
document.querySelector("#actions .start").onclick = function () {
    myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.ADDED));
};
document.querySelector("#actions .cancel").onclick = function () {
    myDropzone.removeAllFiles(true);
};
//Datetimepicker + Mask:
$("#NgayApDung").datetimepicker({
    format: "yyyy/MM/DD",
});
$("#NgayCaiDat").datetimepicker({
    format: "yyyy/MM/DD",
});

ModelId = $("#modalThemphanmemmoi").data("idmodel");
SoftId = $("#modalThemphanmemmoi").data("idphanmem");

function GetModelName(ModelId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Model/GetModelNameFromId",
            type: "GET",
            data: { id: ModelId },
            success: function (response) {
                if (response.success) {
                    var modelName = response.data;
                    resolve(modelName);
                } else {
                    reject("Lỗi khi gọi action.");
                }
            },
            error: function (xhr, status, error) {
                reject("Lỗi khi gọi action: " + error);
            },
        });
    });
}

function GetSoftName(SoftId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Soft/GetSoftNameFromId",
            type: "GET",
            data: { id: SoftId },
            success: function (response) {
                if (response.success) {
                    var softName = response.data;
                    resolve(softName);
                } else {
                    reject("Lỗi khi gọi action.");
                }
            },
            error: function (xhr, status, error) {
                reject("Lỗi khi gọi action: " + error);
            },
        });
    });
}

function GetSoftLastVer(SoftId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Soft/GetSoftLastVersionFromId",
            type: "GET",
            data: { id: SoftId },
            success: function (response) {
                if (response.success) {
                    var softLastVer = response.data;
                    resolve(softLastVer);
                } else {
                    reject("Lỗi khi gọi action.");
                }
            },
            error: function (xhr, status, error) {
                reject("Lỗi khi gọi action: " + error);
            },
        });
    });
}

function GetAllVersionOfSoft(SoftId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Soft/GetAllVersionOfSoft",
            type: "GET",
            data: { id: SoftId },
            success: function (response) {
                if (response.success) {
                    var vers = response.data;
                    resolve(vers);
                } else {
                    reject("Lỗi khi gọi action.");
                }
            },
            error: function (xhr, status, error) {
                reject("Lỗi khi gọi action: " + error);
            },
        });
    });
}

var softAllVers = "";
var softLastVer = "";

Promise.all([
    GetModelName(ModelId),
    GetSoftName(SoftId),
    GetSoftLastVer(SoftId),
    GetAllVersionOfSoft(SoftId),
])
    .then(function (results) {
        var modelName = results[0] || ""; // Gán một giá trị mặc định nếu modelName là null
        var softName = results[1] || ""; // Gán một giá trị mặc định nếu softName là null
        softLastVer = results[2] || ""; // Gán một giá trị mặc định nếu softLastVer là null
        softAllVers = results[3] || "";
        console.log(softAllVers);

        $("#modalThemphanmemmoi .modal-title").text("Tải lên phần mềm mới");
        $("#fUploadTenphanmem").val(softName);
        $("#fUploadTenmodel").val(modelName);
        $("#fUploadVersionHientai").val(softLastVer);
    })
    .catch(function (error) {
        console.error(error);
    });

//Submit form Thêm phần mềm mới:
$("#Themphanmemmoi").submit(function (e) {
    e.preventDefault();
    if (myDropzone.files.length === 0) {
        Swal.fire("Lỗi", "File phần mềm chưa được tải lên", "error");
        return;
    }
    var txtVersion = $("input[name='Version']").val();
    var txtModelName = $("input[name='Model']").val();
    var txtSoftName = $("input[name='Name']").val();
    var txtDiemThayDoi = $("textarea[name='DiemThayDoi']").val();
    var txtNgayApDung = $("input[name='NgayApDung']").val();
    var txtNgayCaiDat = $("input[name='NgayCaiDat']").val();
    var txtSoLuongJigCaiDat = $("input[name='SoLuongJigCaiDat']").val();
    var txtNguoiXacNhan = $("#nguoixacnhanSelect").val();
    var fileName = txtVersion + "_" + nameFile;
    var filePath =
        "uploads" + "\\" + txtModelName + "\\" + txtSoftName + "\\" + fileName;

    if (softAllVers.includes(txtVersion)) {
        Swal.fire(
            "Phiên bản phần mềm đã tồn tại",
            softAllVers.toString(),
            "error"
        );
    } else {
        $.ajax({
            url: "/File/CreateFolder",
            method: "POST",
            data: {
                folderPathModel: txtModelName,
                folderPathSoft: txtSoftName,
            },
            success: function () {
                $.ajax({
                    url: "/File/CutAndPasteFile",
                    method: "POST",
                    data: {
                        modelName: txtModelName,
                        softname: txtSoftName,
                        fileName: nameFile,
                        version: txtVersion,
                    },
                    success: function (response) {
                        if (response.success) {
                            var formData = {
                                Version: txtVersion,
                                DiemThayDoi: txtDiemThayDoi,
                                NgayApDung: txtNgayApDung,
                                NgayCaiDat: txtNgayCaiDat,
                                SoLuongJigCaiDat: txtSoLuongJigCaiDat,
                                NguoiXacNhan: txtNguoiXacNhan,
                                FileName: fileName,
                                Path: filePath,
                                SoftNameId: SoftId,
                            };
                            console.log(formData);
                            $.ajax({
                                url: "/File/UploadInfoSoft",
                                type: "POST",
                                contentType: "application/json",
                                data: JSON.stringify(formData),
                                success: function (response) {
                                    if (response.success) {
                                        Swal.fire(
                                            "Thành công",
                                            "Bạn vừa thêm phần mềm mới thành công",
                                            "success"
                                        ).then(function () {
                                            window.location.reload();
                                        });
                                    }
                                },
                                error: function (xhr, status, error) {
                                    alert("Lỗi khi gửi yêu cầu: " + error);
                                },
                            });
                        } else {
                            Swal.fire(
                                "Không chuyển được file tới thư mục đích",
                                response.message,
                                "error"
                            );
                        }
                    },
                    error: function (xhr, status, error) {
                        Swal.fire(
                            "Không chuyển được file tới thư mục đích",
                            error,
                            "error"
                        );
                    },
                });
            },
            error: function (xhr, status, error) {
                Swal.fire("Không tạo được thư mục mới!", error, "error");
            },
        });
    }
});
