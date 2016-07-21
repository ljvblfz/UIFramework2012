using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;


namespace ComponentArt.Web.UI
{
  class UploadParser
  {
    private const int BUFFER_SIZE = 32768;
    private const int BUFFER_OVERLAP_SIZE = 1024;
    private const int TOTAL_BUFFER_SIZE = BUFFER_SIZE + BUFFER_OVERLAP_SIZE;

    private UploadedFileInfo _currentFile = null;
    private byte[] _buffer;
    private int _currentOffset;
    private long _totalSize;
    private bool _bufferInitialized = false;

    private Random _randomizer = new Random();


    public void StartProcess(long totalSize)
    {
      _buffer = new byte[TOTAL_BUFFER_SIZE];
      Array.Clear(_buffer, 0, TOTAL_BUFFER_SIZE);

      _currentOffset = 0;
      _totalSize = totalSize;
    }

    private void WriteChunk(UploadedFileInfo fileInfo)
    {
      int chunkSize = _bufferInitialized ? BUFFER_SIZE : BUFFER_SIZE - BUFFER_OVERLAP_SIZE;

      if (_currentOffset < fileInfo.LastWriteOffset || fileInfo.EndOffset == fileInfo.StartOffset || fileInfo.StartOffset >= _currentOffset + chunkSize)
      {
        return;
      }

      if (fileInfo.FileStream == null)
      {
        fileInfo.FileStream = new FileStream(fileInfo.TempFileName, FileMode.CreateNew, FileAccess.Write);
      }

      int bufStart = fileInfo.StartOffset > _currentOffset ? (int)(fileInfo.StartOffset - _currentOffset) : (int)fileInfo.LastWriteOffset - _currentOffset;
      int bufEnd = fileInfo.EndOffset > 0 ? (int)(fileInfo.EndOffset - _currentOffset) : chunkSize;

      fileInfo.FileStream.Write(_buffer, bufStart, bufEnd - bufStart);

      fileInfo.LastWriteOffset = _currentOffset + bufEnd;
    }

    private void FinishWrite(UploadedFileInfo fileInfo)
    {
      if (fileInfo.FileStream == null)
      {
        fileInfo.FileStream = new FileStream(fileInfo.TempFileName, FileMode.CreateNew, FileAccess.Write);
      }

      // do we need to write anything from the current chunk?
      if (_currentOffset >= fileInfo.LastWriteOffset)
      {
        int chunkSize = _bufferInitialized ? BUFFER_SIZE + BUFFER_OVERLAP_SIZE: BUFFER_SIZE;

        // did the file start in this chunk?
        int bufStart = (fileInfo.StartOffset >= _currentOffset && fileInfo.StartOffset < _currentOffset + chunkSize) ? (int)(fileInfo.StartOffset - _currentOffset) : (int)fileInfo.LastWriteOffset - _currentOffset;
        int bufEnd = fileInfo.EndOffset < _currentOffset + chunkSize ? (int)(fileInfo.EndOffset - _currentOffset) : chunkSize;

        fileInfo.FileStream.Write(_buffer, bufStart, bufEnd - bufStart);
      }

      fileInfo.FileStream.Flush();
      fileInfo.FileStream.Close();
      fileInfo.FileStream = null;
    }

    public void ProcessChunk(UploadInfo oUploadInfo, byte[] arBuffer, int iSize)
    {
      // update progress
      oUploadInfo.Progress = (double)_currentOffset / _totalSize;
      
      if (_bufferInitialized)
      {
        if (iSize < BUFFER_SIZE)
        {
          byte[] overlappedBuff = new byte[BUFFER_OVERLAP_SIZE];
          
          Array.Copy(_buffer, BUFFER_SIZE, overlappedBuff, 0, BUFFER_OVERLAP_SIZE);
          
          Array.Clear(_buffer, 0, BUFFER_SIZE);
          
          Array.Copy(overlappedBuff, 0, _buffer, 0, BUFFER_OVERLAP_SIZE);
          
          Array.Copy(arBuffer, 0, _buffer, BUFFER_OVERLAP_SIZE, iSize);
        }
        else
        {
          // copy overlap from the end of the previous buffer to the front 
          Array.Copy(_buffer, BUFFER_SIZE, _buffer, 0, BUFFER_OVERLAP_SIZE);

          // add new data 
          Array.Copy(arBuffer, 0, _buffer, BUFFER_OVERLAP_SIZE, BUFFER_SIZE);
        }
      }
      else
      {
        // copy data to beginning of buffer
        Array.Copy(arBuffer, 0, _buffer, 0, BUFFER_SIZE);

        // duplicate overlap bit so it will be copied next time
        Array.Copy(arBuffer, BUFFER_SIZE - BUFFER_OVERLAP_SIZE, _buffer, BUFFER_SIZE, BUFFER_OVERLAP_SIZE);
      }

      string sChunk = Encoding.ASCII.GetString(_buffer, 0, _bufferInitialized? TOTAL_BUFFER_SIZE : BUFFER_SIZE);

      int iBoundaryIndex = sChunk.IndexOf(oUploadInfo.Boundary);

      if (iBoundaryIndex >= 0)
      {
        string sLowerChunk = sChunk.ToLower();

        while (iBoundaryIndex >= 0)
        {
          // do we need to finish a file in progress?
          if (_currentFile != null)
          {
            if (_currentOffset + iBoundaryIndex > _currentFile.StartOffset)
            {
              _currentFile.EndOffset = _currentOffset + iBoundaryIndex - "\r\n".Length;
              _currentFile.Size = _currentFile.EndOffset - _currentFile.StartOffset;
              oUploadInfo.UploadedFiles.Add(_currentFile);
              
              FinishWrite(_currentFile);

              _currentFile = null;
            }
            else
            {
              break;
            }
          }

          int iNextBoundary = sChunk.IndexOf(oUploadInfo.Boundary, iBoundaryIndex + oUploadInfo.Boundary.Length);

          // do we have enough info for a new file?
          int iFileNameIndex = sLowerChunk.IndexOf("filename=\"", iBoundaryIndex);
          int iContentTypeIndex = sLowerChunk.IndexOf("content-type: ", iBoundaryIndex);

          if (iFileNameIndex > iBoundaryIndex && iContentTypeIndex > iFileNameIndex && (iNextBoundary < 0 || iFileNameIndex < iNextBoundary))
          {
            // find data start
            int iDataIndex = sLowerChunk.IndexOf("\r\n\r\n", iContentTypeIndex);

            if (iDataIndex > iContentTypeIndex)
            {
              // does this put us over our file limit?
              if (oUploadInfo.UploadedFiles.Count + 1 > oUploadInfo.MaximumFiles)
              {
                throw new Exception("Too many files.");
              }

              // Create file info
              _currentFile = new UploadedFileInfo();

              // we need to make sure the file name is processed as UTF8
              int iFileNameStartOffset = iFileNameIndex + "filename=\"".Length;
              _currentFile.FileName = Encoding.UTF8.GetString(_buffer, iFileNameStartOffset, (_bufferInitialized ? TOTAL_BUFFER_SIZE : BUFFER_SIZE) - iFileNameStartOffset);
              _currentFile.FileName = _currentFile.FileName.Substring(0, _currentFile.FileName.IndexOf("\""));

              // get rid of client path
              if (_currentFile.FileName.IndexOf("\\") >= 0)
              {
                _currentFile.FileName = _currentFile.FileName.Substring(_currentFile.FileName.LastIndexOf("\\") + 1);
              }
              
              _currentFile.StartOffset = _currentOffset + iDataIndex + "\r\n\r\n".Length;

              _currentFile.ContentType = sChunk.Substring(iContentTypeIndex + "content-type: ".Length);
              _currentFile.ContentType = _currentFile.ContentType.Substring(0, _currentFile.ContentType.IndexOf("\r\n"));

              // parse extension
              _currentFile.Extension = "";
              if (_currentFile.FileName.IndexOf('.') > 0)
              {
                int iExtensionIndex = _currentFile.FileName.LastIndexOf('.');
                if (iExtensionIndex < _currentFile.FileName.Length - 1)
                {
                  _currentFile.Extension = _currentFile.FileName.Substring(iExtensionIndex + 1);
                }
              }

              _currentFile.TempFileName = oUploadInfo.TempFolder + "\\" + CreateTempFileName(_currentFile);
              oUploadInfo.CurrentFile = _currentFile.FileName;
            }
          }

          iBoundaryIndex = iNextBoundary; 
        }
      }

      if (_currentFile != null)
      {
        if (iSize < BUFFER_SIZE)
        {
          _currentFile.EndOffset = _currentOffset + iSize;
          _currentFile.Size = _currentFile.EndOffset - _currentFile.StartOffset;

          FinishWrite(_currentFile);

          _currentFile = null;
        }
        else
        {
          WriteChunk(_currentFile);
        }
      }

      _currentOffset += _bufferInitialized ? BUFFER_SIZE : BUFFER_SIZE - BUFFER_OVERLAP_SIZE;

      _bufferInitialized = true;
    }

    public void FinishProcess(UploadInfo oUploadInfo)
    {
      oUploadInfo.Progress = 1;
    }

    private string CreateTempFileName(UploadedFileInfo oFileInfo)
    {
      return string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMddHHmmssffff"), _randomizer.Next(), oFileInfo.FileName);
    }
  }
}
