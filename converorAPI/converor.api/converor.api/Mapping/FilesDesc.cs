using converor.api.Dtos.Files;
using converor.Core.Models;

namespace converor.api.Mapping
{
    public static class FilesDesc
    {
        public static List<GetFileDescDto> ToGetFileDescDto(this List<FileDescription> files){
            List<GetFileDescDto> filesDto = [];
            foreach(var file in files)
                filesDto.Add(new (){Id = file.Id, Name = file.FileName, UpdatedTime = file.UploadedDate});
            return filesDto;
        }
    }
}