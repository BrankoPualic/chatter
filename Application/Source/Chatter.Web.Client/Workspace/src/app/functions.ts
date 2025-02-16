import { cloneDeep } from "lodash";
import { INameofOptions } from "./models/function-options.model";
import { api } from "./_generated/project";

export class Functions {
  static nameof<T extends object>(
    exp: ((obj: T) => any) | (new (...params: any[]) => T),
    options?: INameofOptions,
  ): string {
    const fnStr = exp.toString();

    if (fnStr.substring(0, 6) == 'class ' && fnStr.substring(0, 8) != 'class =>') {
      return this.cleanseAssertionOperators(fnStr.substring('class '.length, fnStr.indexOf(' {')));
    }

    if (fnStr.indexOf('=>') !== -1) {
      let name = this.cleanseAssertionOperators(fnStr.substring(fnStr.indexOf('.') + 1));
      if (options?.lastPart) name = name.substring(name.lastIndexOf('.') + 1);
      return name;
    }

    throw new Error('ts-simple-nameof: Invalid function');
  }

  private static cleanseAssertionOperators(parsedName: string): string {
    return parsedName.replace(/[?!]/g, '');
  }

  // Object
  static clone = <T extends object>(model?: T): T => !!model ? cloneDeep(model) : {} as T;

  // String
  static formatString = (text?: string): string => !!text ? text.replace(/\n/g, '<br/>') : '';

  // Date
  static formatRequestDates(data: any): void {
    // Ignore things that aren't objects.
    if (typeof data !== 'object')
      return data;

    for (let key in data) {
      if (data.hasOwnProperty(key)) {
        const value = data[key];

        if (value instanceof Date) {
          data[key] = value.toISOString();
        } else if (typeof value === 'object') {
          this.formatRequestDates(value);
        }
      }
    }
  }

  // File
  static readFileUrl(file: File): Promise<string> {
    if (file.type.startsWith('video/'))
      return this.readVideoUrl(file)
    else
      return this.readImageUrl(file);
  }

  private static readImageUrl(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => { resolve(reader.result as string); };
      reader.onerror = (error) => { reject(error); };
      reader.readAsDataURL(file);
    });
  }

  private static readVideoUrl(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const video = document.createElement("video");
      video.preload = "auto"; // Preload the video for better performance
      video.src = URL.createObjectURL(file);

      // Wait until the video metadata (duration, dimensions) is loaded
      video.onloadedmetadata = () => {
        const canvas = document.createElement("canvas");
        const context = canvas.getContext("2d");

        // Ensure the video is loaded
        if (context) {
          canvas.width = video.videoWidth;
          canvas.height = video.videoHeight;

          // Draw the first frame on the canvas
          video.currentTime = 0; // Seek to the start of the video

          // Wait for the video to be ready at the desired current time
          video.onseeked = () => {
            context.drawImage(video, 0, 0, canvas.width, canvas.height);
            const thumbnail = canvas.toDataURL("image/png"); // Create the image data
            resolve(thumbnail);
          };
        } else {
          reject(new Error("Failed to get canvas context"));
        }
      };

      video.onerror = () => {
        reject(new Error("Error loading video file"));
      };
    });
  }

  static formatFileSize(size: number): string {
    if (size < 1024)
      return `${size} B`;

    let units = ["KB", "MB", "GB", "TB"];
    let i = -1;

    do {
      size /= 1024;
      i++;
    } while (size >= 1024 && i < units.length - 1);

    return `${size.toFixed(1)} ${units[i]}`;
  }

  static getVideoDuration(file: File): Promise<number> {
    return new Promise((resolve, reject) => {
      const video = document.createElement('video');
      video.preload = 'metadata';

      video.onloadedmetadata = () => {
        window.URL.revokeObjectURL(video.src);
        resolve(video.duration);
      };

      video.onerror = (error) => { reject(error) };

      const reader = new FileReader();
      reader.onload = (event) => {
        if (event.target?.result)
          video.src = event.target.result as string;
      };
      reader.readAsDataURL(file);
    })
  }

  // JSON
  static toJson(data: any): string {
    try {
      return JSON.stringify(data, Functions.removeCircularReferences());
    } catch (error) {
      console.error('Error serializing object:', error);
      return '{}';
    }
  }

  static generateId = (): string => Math.random().toString(36).substring(2, 8 + Math.floor(Math.random() * 5));

  static getEnum = (referenceService: api.Providers, provider: string, exp: (_: api.EnumProvider) => boolean): api.EnumProvider | undefined => {
    const methodName = `get${provider}` as keyof api.Providers;

    const providerMethod = referenceService[methodName] as (() => api.EnumProvider[]) | undefined;

    return typeof providerMethod === 'function'
      ? providerMethod().find(exp)
      : undefined;
  }

  // private

  private static removeCircularReferences() {
    const seen = new WeakSet();
    return (key: any, value: any) => {
      if (typeof value === 'object' && value !== null) {
        if (seen.has(value))
          return '[Circular]';
        seen.add(value);
      }
      return value;
    }
  }
}
